using Collapse.Code.Model;
using System;
using System.Collections.Generic;
using System.Text;

public class CardTitleGenerator
{
    private const byte minPartsQuantity = 1;
    private const byte maxPartsQuantity = 3;
    private const byte addThirdPartChance = 50; 
    private const int addWordEndingsChance = 70;
    private const byte hundrerPercent = 100;

    private static readonly List<string> FirstPart = new List<string>()
    {
        "Вор", "Зар", "Кор", "Дра", "Мор", "Тар", "Гри", "Фен", "Сар", "Хар",
        "Бел", "Чер", "Огн", "Лун", "Сол", "Тен", "Яр", "Вел", "Кос", "Рок",
        "Вал", "Гром", "Снеж", "Пла", "Ту", "Щит", "Меч", "Клин", "Бо", "Ве",
        "Зла", "Сил", "Тай", "Мир", "Кра", "Гор", "Лес", "Вет", "Дре", "Ска",
        "Жар", "Лед", "Тьм", "Свет", "Кров", "Пес", "Ка", "Ба", "Ра", "Да"
    };

    private static readonly List<string> SecondPart = new List<string>()
    {
        "то", "ра", "ни", "во", "ме", "ко", "ли", "ну", "са", "де",
        "га", "ла", "фа", "ши", "чу", "бя", "рю", "зо", "бу", "гло",
        "бо", "до", "ро", "го", "мо", "по", "со", "хо", "ло", "йо",
        "ви", "ди", "пи", "ки", "ти", "ри", "ми", "си", "ци", "би",
        "ве", "зе", "пе", "ке", "те", "ре", "ще", "же", "че", "фе"
    };

    private static readonly List<string> ThirdPart = new List<string>()
    {
        "дон", "тир", "вий", "зан", "кос", "тас", "рин", "дис", "зор", "тал",
        "гар", "фис", "нур", "мис", "бур", "лак", "гун", "вих", "дур", "пас",
        "мар", "вер", "дер", "тер", "пер", "сер", "мер", "бер", "гер", "кер",
        "тор", "бор", "мор", "вор", "дор", "кор", "рор", "сор", "фор", "шор",
        "тан", "ван", "ман", "бан", "дан", "ран", "сан", "фан", "чан", "жан"
    };

    private static readonly List<string> WordEndings = new List<string>()
    {
        "а", "о", "е", "ий", "ай", "ей", "я",
        "ик", "ок", "ек", "ук", "юк", "як", "ич", "ач", "еч", "оч"
    };

    public static string Generate()
    {
        var partsQuantity = GameModel.Random.Next(minPartsQuantity, maxPartsQuantity + 1);
        var word = new StringBuilder();
        word.Append(GetRandomElement(FirstPart));

        for (var i = 1; i < partsQuantity; i++)
            word.Append(GetRandomElement(SecondPart));

        if (ShouldAddElement(addThirdPartChance))
            word.Append(GetRandomElement(ThirdPart));

        if (ShouldAddElement(addWordEndingsChance))
            word.Append(GetRandomElement(WordEndings));
        return word.ToString();
    }

    private static bool ShouldAddElement(byte percentageChance) => GameModel.Random.Next(hundrerPercent) < percentageChance;

    private static T GetRandomElement<T>(List<T> list) => 
        list[GameModel.Random.Next(list.Count)];
}