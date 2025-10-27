using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using Core.Interfaces;
using System.Net.NetworkInformation;

namespace Core.Services;

public class ImageService(IConfiguration configuration) : IImageService
{
    public async Task DeleteImageAsync(string name)
    {
        //читає список розмірів, для яких збережені версії зображення
        var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>();
        //отримує директорію збереження зображень і будує абсолютний шлях
        var dir = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImagesDir"]!);

        //для кожного розміру створюється окреме завдання (Task)
        Task[] tasks = sizes
            .AsParallel() //дозволяє виконувати ці завдання одночасно
            .Select(size =>
            {
                return Task.Run(() =>
                {
                    var path = Path.Combine(dir, $"{size}_{name}");
                    if (File.Exists(path)) //перевіряє існування файлу
                    {
                        File.Delete(path);
                    }
                });
            })
            .ToArray();

        //асинхронне очікування завершення всіх задач
        await Task.WhenAll(tasks);
    }

    public async Task<string> SaveImageFromUrlAsync(string imageUrl)
    {
        //створюється новий Http клієнт.
        //using означає, що після виконання методу клієнт буде автоматично закритий і звільнений з пам’яті
        using var httpClient = new HttpClient();

        //виконується HTTP-запит GET за адресою imageUrl
        //повертається байтовий масив, який містить вміст файлу (зображення), отриманого з цього URL
        var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

        //зображення зберігається локально у форматі .webp та повертається ім’я збереженого файлу
        return await SaveImageAsync(imageBytes);
    }

    //основний вхідний пункт для прийому зображення, яке користувач завантажує через форму або API-запит
    public async Task<string> SaveImageAsync(IFormFile file)
    //IFormFile — це стандартний тип ASP.NET Core, який представляє файл, надісланий через HTTP-запит
    //повертає string — тобто ім’я збереженого файлу
    {
        // перевірка: якщо користувач не передав зображення — просто повертаємо null
        if (file == null || file.Length == 0)
            return null;

        using MemoryStream ms = new(); //створюється тимчасовий потік у пам’яті
        await file.CopyToAsync(ms); //копіює вміст завантаженого файлу у цей потік
        var bytes = ms.ToArray(); //отримує масив байтів із цього потоку

        //Метод делегує обробку іншому перевантаженню SaveImageAsync(byte[] bytes)
        //Той, у свою чергу, створює унікальне ім’я файлу і зберігає копії зображення у різних розмірах
        var imageName = await SaveImageAsync(bytes);

        //повертає назву базового файлу
        return imageName;
    }

    private async Task<string> SaveImageAsync(byte[] bytes)
    {
        //генерує випадкове ім’я файлу
        string imageName = $"{Path.GetRandomFileName()}.webp";
        //з конфігураційного файлу отримується список розмірів, у яких треба зберегти зображення
        var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>();

        //для кожного розміру створюється окреме завдання(Task)
        Task[] tasks = sizes
            .AsParallel() //дозволяє виконувати ці завдання одночасно
            .Select(s => SaveImageAsync(bytes, imageName, s))
            .ToArray();

        //асинхронне очікування завершення всіх задач
        await Task.WhenAll(tasks);

        return imageName; //повертає базову назву файлу
    }

    //метод потрібен, щоб зберегти зображення, яке передано у форматі Base64
    public async Task<string> SaveImageFromBase64Async(string input)
    {
        //Витягує чисті Base64-дані з рядка, який може містити префікс
        var base64Data = input.Contains(",")
           ? input.Substring(input.IndexOf(",") + 1)
           : input;

        //декодує Base64-рядок у масив байтів
        byte[] imageBytes = Convert.FromBase64String(base64Data);

        //створює зображення у різних розмірах, зберігає їх на диск і повертає ім’я збереженого файлу
        return await SaveImageAsync(imageBytes);
    }

    //метод бере сирі байти зображення, створює його окрему копію певного розміру
    //і зберігає її у форматі.webp у потрібну папку
    private async Task SaveImageAsync(byte[] bytes, string name, int size)
    {
        //формування шляху для збереження
        var path = Path.Combine(
            Directory.GetCurrentDirectory(), //коренева папка проєкту, де запускається сервер
            configuration["ImagesDir"]!, //назва директорії для зображень
            $"{size}_{name}");

        //завантаження зображення з байтів
        //цей виклик використовує бібліотеку ImageSharp.
        //вона автоматично визначає формат зображення(jpg, png, gif, тощо).
        //тепер image — це об’єкт типу Image, який можна змінювати
        using var image = Image.Load(bytes);

        //зміна розміру зображення
        image.Mutate(imgConext => //застосовує певні операції до зображення
        {
            imgConext.Resize(new ResizeOptions
            {
                //бажаний розмір
                Size = new Size(size, size),
                //зображення зменшується або збільшується, щоб поміститися в квадрат із заданим розміром
                //зберігає пропорції, тобто не спотворює зображення
                Mode = ResizeMode.Max
            });
        });

        //збереження зображення у форматі .webp за вказаним шляхом
        await image.SaveAsync(path, new WebpEncoder());
    }
}
