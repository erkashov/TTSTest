// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Sqlite;
using System;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Text;
using TTSTest.OneDB;
using TTSTest.TemplateDB;
using TTSTest.TwoDB;

var templateContext = new TemplateDbContext();
var oneDbContext = new Db1Context();
var twoContext = new Db2Context();

templateContext.Database.EnsureCreated();
oneDbContext.Database.EnsureCreated();
twoContext.Database.EnsureCreated();

var log = new StringBuilder();
//переносим типы компонентов в главную БД
foreach (var type in oneDbContext.ComponentTypes)
{
    //признак уникальности - название
    if (!templateContext.ComponentTypes.Any(p => p.Type == type.Type))
    {
        templateContext.ComponentTypes.Add(new TTSTest.TemplateDB.ComponentType() { Type = type.Type });
        log.AppendLine($"DB1\tДобавлен тип компонента {type.Type}");
    }
    else
    {
        log.AppendLine($"DB1\tТип компонента {type.Type} пропущен");
    }
}

//сохраняем айдишники компонентов в главной бд
//нужно только если мы будем переносить несколько раз, иначе не нужен
var newComponentsIds = new Dictionary<int, int>();

foreach (var component in oneDbContext.Components)
{
    var typeId = templateContext.ComponentTypes.FirstOrDefault(p => p.Type == component.Type.Type)?.Id ?? 0;
    //ищем компонент в главной БД
    //признак уникальности - наименование в связке с типом
    var oldComp = templateContext.Components.FirstOrDefault(p => p.Name == component.Name && p.TypeId == typeId);
    if (oldComp == null)
    {
        var comp = new TTSTest.TemplateDB.Component() { Name = component.Name, TypeId = typeId };

        templateContext.Components.Add(comp);
        templateContext.SaveChanges();
        newComponentsIds.Add(comp.Id, component.Id);
        log.AppendLine($"DB1\tДобавлен компонент {comp.Name}");
    }
    else
    {
        //если компонент уже добавлен, просто сохраняем его айди
        newComponentsIds.Add(oldComp.Id, oldComp.Id);
        log.AppendLine($"DB1\tКомпонент {component.Type} пропущен");
    }
}

//сохраняем айдишники рецептов в главной бд
//нужно только если мы будем переносить несколько раз
var newReceiptsIds = new Dictionary<int, int>();

foreach (var recipe in oneDbContext.Recipes)
{
    //ищем рецепт в главной БД
    //признак уникальности - наименование (если есть какой то еще признак, указываем его)
    var oldRecipe = templateContext.Recipes.Where(p => p.Name == recipe.Name).FirstOrDefault();
    if (oldRecipe == null)
    {
        TTSTest.TemplateDB.Recipe rec = new TTSTest.TemplateDB.Recipe()
        {
            Name = recipe.Name,
            DateModified = recipe.DateModified
        };
        //MixerSetId не знаю как маппить, может как то связан с water_correct, но связи не нашел
        rec.TimeSetId = templateContext.RecipeTimeSets.FirstOrDefault(p => p.MixTime == recipe.MixTime)?.Id;
        templateContext.Recipes.Add(rec);
        templateContext.SaveChanges();
        newReceiptsIds.Add(rec.Id, recipe.Id);
        log.AppendLine($"DB1\tДобавлен рецепт {recipe.Name}");
    }
    else
    {
        //если компонент уже добавлен, просто сохраняем его айди
        newReceiptsIds.Add(oldRecipe.Id, oldRecipe.Id);
        log.AppendLine($"DB1\tРецепт {recipe.Name} пропущен");
    }
}

foreach (var recipeStructure in oneDbContext.RecipeStructures)
{
    //ищем стурктуру рецепта в главной БД
    //признак уникальности - id рецепта и компонента
    //находим id, по которому мы сохранили сущность

    //Предложите возможные решения, если структура рецепта recipe_structure после переноса должна измениться из-за разницы в количестве дозирующих бункеров до и после переноса.
    //как понял, надо учитывтаь что изменилось amount
    //тогда 2 решения вижу:
    //1. обновлять amount
    //2. учитывать amount при проверке на уникальность, и просто добавляем с новым amount

    if (!templateContext.RecipeStructures.Any(p => p.RecipeId == newReceiptsIds[recipeStructure.RecipeId] && p.ComponentId == newComponentsIds[recipeStructure.ComponentId]))
    {
        var recipeStr = new TTSTest.TemplateDB.RecipeStructure()
        {
            RecipeId = newReceiptsIds[recipeStructure.RecipeId],
            ComponentId = newComponentsIds[recipeStructure.ComponentId],
            Amount = recipeStructure.Amount
        };
        templateContext.RecipeStructures.Add(recipeStr);
        templateContext.SaveChanges();
        log.AppendLine($"DB1\tДобавлена структура компонента {recipeStructure.Recipe.Name}-{recipeStructure.Component.Name}");
    }
}

//twoContext.MixerSets и twoContext.TimeSets я придумал сам, так как в ТЗ о них не было упоминания в контексте главной БД

//сохраняем айдишники миксеров в главной бд
//нужно только если мы будем переносить несколько раз
var newMixersIds = new Dictionary<int, int>();

foreach (var mixer in twoContext.MixerSets)
{
    //ищем миксер в главной БД
    //признак уникальности - наименование, время выгрузки в секундах, режим выгрузки 
    var oldMixer = templateContext.RecipeMixerSets.Where(p => p.Name == mixer.Name && p.UploadMode == mixer.UploadMode && p.UnloadTime == mixer.UnloadTime).FirstOrDefault();
    if (oldMixer == null)
    {
        var mix = new TTSTest.TemplateDB.RecipeMixerSet()
        {
            Name = mixer.Name,
            UnloadTime = mixer.UnloadTime,
            UploadMode = mixer.UploadMode
        };
        templateContext.RecipeMixerSets.Add(mix);
        templateContext.SaveChanges();
        newMixersIds.Add(mixer.Id, mix.Id);
        log.AppendLine($"DB2\tДобавлен mixer {mixer.Name}");
    }
    else
    {
        newMixersIds.Add(oldMixer.Id, oldMixer.Id);
        log.AppendLine($"DB2\tMixer {mixer.Name} пропущен");
    }
}

//сохраняем айдишники миксеров в главной бд
//нужно только если мы будем переносить несколько раз
var newTimeSetsIds = new Dictionary<int, int>();

foreach (var timeSet in twoContext.TimeSets)
{
    //ищем время в главной БД
    //признак уникальности - наименование, время смешивания бетона в секундах  
    var oldMixer = templateContext.RecipeTimeSets.Where(p => p.Name == timeSet.Name && p.MixTime == timeSet.MixTime).FirstOrDefault();
    if (oldMixer == null)
    {
        var time = new TTSTest.TemplateDB.RecipeTimeSet()
        {
            Name = timeSet.Name,
            MixTime = timeSet.MixTime,
        };
        templateContext.RecipeTimeSets.Add(time);
        templateContext.SaveChanges();
        newTimeSetsIds.Add(timeSet.Id, time.Id);
        log.AppendLine($"DB2\tДобавлен timeSet {timeSet.Name}");
    }
    else
    {
        newTimeSetsIds.Add(oldMixer.Id, oldMixer.Id);
        log.AppendLine($"DB2\tTimeSet {timeSet.Name} пропущен");
    }
}

//Тут еще должна быть таблица Consistencies, но честно не понял как она спроектирована, поэтому у нее только id и наименование

foreach (var recipe in twoContext.Recipes)
{
    var oldRecipe = templateContext.Recipes.Where(p => p.Name == recipe.Name).FirstOrDefault();
    if (oldRecipe == null)
    {
        TTSTest.TemplateDB.Recipe rec = new TTSTest.TemplateDB.Recipe()
        {
            Name = recipe.Name,
            DateModified = recipe.DateModified
        };
        if (recipe.TimeSetId.HasValue) rec.TimeSetId = newTimeSetsIds[recipe.TimeSetId.Value];
        if (recipe.MixerSetId.HasValue) rec.MixerSetId = newMixersIds[recipe.MixerSetId.Value];
        templateContext.Recipes.Add(rec);
        templateContext.SaveChanges();
        log.AppendLine($"DB2\tДобавлен рецепт {recipe.Name}");
    }
    else
    {
        log.AppendLine($"DB2\tРецепт {recipe.Name} пропущен");
    }
}
