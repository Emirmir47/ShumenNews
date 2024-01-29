using Microsoft.AspNetCore.Identity;
using NuGet.Protocol;
using ShumenNews.Data.Models;

namespace ShumenNews.Data.Seeding
{
    public class ShumenNewsSeeder : ISeeder
    {
        private readonly ShumenNewsDbContext db;

        public ShumenNewsSeeder(ShumenNewsDbContext db)
        {
            this.db = db;
        }

        public async Task Seed()
        {
            if (!db.Articles.Any())
            {
                //Users
                var user = new ShumenNewsUser
                {
                    UserName = "root",
                    NormalizedUserName = "ROOT",
                    Email = "root@shumen_news.com",
                    NormalizedEmail = "ROOT@SHUMEN_NEWS.COM",
                    FirstName = "Root",
                    LastName = "Root",
                    EmailConfirmed = true
                };
                await db.Users.AddAsync(user);


                //Articles
                var article = new ShumenNewsArticle
                {
                    Title = "Хванаха 41-годишен шуменец да кара Аудито си с 1,32 промила алкохол!",
                    Content = "Установен е 41-годишен мъж от Шумен, който снощи управлявал автомобил след употреба на алкохол, " +
                    "съобщиха от пресцентъра на ОД на МВР.\r\n\r\nОколо 23,40 часа на бул.” " +
                    "Симеон Велики“ в Шумен полицаи спрели за проверка  Ауди А4. " +
                    "Тествали водача с дрегер, който отчел 1,32 промила алкохол в издишания въздух.\r\n" +
                    "Мъжът дал кръвна проба за анализ, след което бил задържан за срок до 24 часа в полицейското управление в Шумен.",
                    Likes = 150,
                    Dislikes = 10,
                    PublishedOn = DateTime.UtcNow,
                    Views = 1500,
                    MainImageId = "seederImg1"
                };
                

                var article2 = new ShumenNewsArticle
                {
                    Title = "57 293 кубика дърва за огрев на населението в Шуменско са предоставили от СИДП!",
                    Content = "12 821 семейства в област Шумен се отопляват с дърва през тази зима, като те вече са получили и заплатили 57 293 пространствени кубически " +
                    "метра дърва за огрев от Североизточното държавно предприятие (СИДП)," +
                    " съобщиха от пресцентъра на предприятието.В областния център домакинствата на дърва са 2242, " +
                    "а кубиците са 12 199. В Нови пазар 3131 семейства са избрали да се отопляват 15 467 кубика дърва, а в Смядово те са 1147, " +
                    "като получените от тях дърва за огрев са 5732 кубика.  " +
                    "\r\n\r\nНа територията на ДЛС-Паламара осигурените дърва са 20 908 кубика за " +
                    "3587 домакинства, във Велики Преслав са 11 420 кубика за 1881 желаещи и във Върбица са 7034 за 833 семейства." +
                    "\r\n\r\nОбщо 200 000 пространствени кубически метра дърва за огрев са предоставили през 2023 г. " +
                    "стопанствата от системата на СИДП на населението, " +
                    "\r\n\r\nНай-много дърва за хората са осигурили от ДЛС-Паламара – 20 908 кубика," +
                    "следвани от горските стопанства във Варна и Търговище.",
                    Likes = 2350,
                    Dislikes = 24,
                    PublishedOn = DateTime.UtcNow,
                    Views = 25000,
                    MainImageId = "seederImg4"
                };
                var article3 = new ShumenNewsArticle
                {
                    Title = "Задържаха помагач на телефонни измамници!",
                    Content = "37-годишен мъж от Шумен, попаднал в схема на телефонна измама, е задържан от полицията, съобщиха от пресцентъра на ОД на МВР." +
                    "\r\n\r\nМъжът, който бил нает за куриер е установен за по-малко от 24 часа криминалистите. " +
                    "Работата започнала, след като в обедните часове във вторник 87-годишен мъж съобщил в РУ - Шумен, " +
                    "че е измамен по телефона.Обяснил, че предния ден бил потърсен от непознат, който се представил за началника на полицията в града. " +
                    "Съобщил му, че се провежда акция за залавяне на телефонни измамници и го помолил да съдейства и за целта трябва да остави парична сума, " +
                    "която измамниците като вземат да бъдат задържани на място и да бъдат осъдени. Обещали му, че след акцията парите ще му бъдат върнати.\r\n\r\n" +
                    "С мисълта, че помага на полицията мъжът оставил сума в размер на 11 000 лева на определено място на ул. “Ген. Гурко“ в Шумен. След като оставил парите се прибрал." +
                    " На следващия ден съобщил в полицейското управление за случилото се. Образувано е досъдебно производство за извършено престъпление по чл. 209 ал. 1 от НК. " +
                    "Криминалисти от полицейското управление в града предприели редица действия за установяване на извършителя. Влезли в дирите на 37-годишен мъж от Шумен. " +
                    "Предприетите мерки дали резултат и часове по-късно мъжът бил установен и разпитан. Пред полицейските служители обяснил, че бил нает от непознати по телефона, " +
                    "за да извърши куриерска дейност. Трябвало да вземе от определено място един пакет и да го занесе до друго. За услугата щял да получи добро възнаграждение." +
                    "\r\n\r\nРазпитан е пред съдия, повдигнато му е обвинение във връзка с извършеното престъпление. Работата по документиране на случая продължава.",
                    Likes = 1000,
                    Dislikes = 400,
                    PublishedOn = DateTime.UtcNow,
                    Views = 2500,
                    MainImageId = "seederImg5"
                };
                var article4 = new ShumenNewsArticle
                {
                    Title = "Мъж с Тойота удари 5 паркирани коли в Шумен!",
                    Content = "Водач на лек автомобил Тойота Авенсис е ударил тази сутрин 5 паркирани на бул. " +
                    "„Симеон Велики“ коли. Инцидентът станал в 8,20 часа пред Професионалната гимназия по икономика, научи ШУМ.БГ.\r\n\r\n" +
                    "Тойотата се движила по булеварда в западна посока. Според очевидци, на кръстовището пред НОИ водачът форсирал силно" +
                    " двигателя, колата ускорила бързо, след което ударила последователно 5 паркирани коли. Спряла се в Субару Легаси," +
                    " което пострадало най-сериозно.\r\n\r\nНа мястото пристигна екип на полицията, видя репортер на ШУМ.БГ. Водачът беше " +
                    "тестван за алкохол и наркотици, като пробите му бяха отрицателни.\r\n\r\nОт пресцентъра на полицията обясниха по-късно," +
                    "че водачът е казал, че се е подхлъзнал на пътя.",
                    
                    Likes = 700,
                    Dislikes = 100,
                    PublishedOn = DateTime.UtcNow,
                    Views = 951,
                    MainImageId = "seederImg6"
                };
                var article5 = new ShumenNewsArticle
                {
                    Title = "684 деца са родени в МБАЛ-Шумен през миналата година!",
                    Content = "През миналата година в МБАЛ-Шумен са родени 684 деца. Броят им е по-нисък от този през 2022-ра, " +
                    "съобщи пред журналисти днес началникът на Неонатологичното отделение д-р Лиляна Куздова.\r\n\r\n" +
                    "„По-малко са родените през 2023-та година. Живородените деца са 684, като 80 са недоносени. " +
                    "Нямаме обаче екстремно недоносени деца и това е добре“, каза д-р Куздова. " +
                    "Процентът на недоносените е малко по-висок от средния за страната, но това според нея се дължи на спецификата на населението в региона." +
                    "\r\n\r\n„Препоръките ни са за раждане на повече от едно дете в семейство. Също така в никакъв случай да не се отлага моментът на " +
                    "първото раждане, което е една от причините за високия процент на недоносените деца“, каза още началникът на Неонатологично." +
                    "\r\n\r\nПрез 2022-ра в шуменската болница се родиха 782 бебета, или с 98 повече спрямо 2023-та. Спадът е с близо 13%.",
                   
                    Likes = 990,
                    Dislikes = 0,
                    PublishedOn = DateTime.UtcNow,
                    Views = 2540,
                    MainImageId = "seederImg12"
                };
                var article6 = new ShumenNewsArticle
                {
                    Title = "Мъж въртя Опел през таван на пътя Нови пазар-Стоян Михайловски!",
                    Content = "Мъж от село Ружица катастрофира около 8,00 часа тази сутрин. Инцидентът стана на пътя Нови пазар - Стоян Михайловски, " +
                    "съобщиха от пресцентъра на полицията.\r\n\r\nНа заледен участък Опелът поднесъл и излязъл от платното, като се обърнал няколко пъти " +
                    "през таван.\r\n\r\nЗа щастие 51-годишният водач се отървал без никакви наранявания. Тестът му за алкохол е отрицателен.",
                     
                    Likes = 1000,
                    Dislikes = 200,
                    PublishedOn = DateTime.UtcNow,
                    Views = 1504,
                    MainImageId = "seederImg13"
                };
                db.Articles.Add(article);
                db.Articles.Add(article2);
                db.Articles.Add(article3);
                db.Articles.Add(article4);
                db.Articles.Add(article5);
                db.Articles.Add(article6);
                    

                //UserArticles
                var userArticle = new ShumenNewsUserArticle
                {
                    User = user,
                    Article = article,
                    IsAuthor = true
                };
                db.UserArticles.Add(userArticle);
                var userArticle2 = new ShumenNewsUserArticle
                {
                    User = user,
                    Article = article2,
                    IsAuthor = true
                };
                db.UserArticles.Add(userArticle2);

                //Images
                ShumenNewsImage image = new ShumenNewsImage
                {
                    Id = "seederImg1",
                    Extension = "jpg",
                    Article = article
                };
                ShumenNewsImage image2 = new ShumenNewsImage
                {
                    Id = "seederImg2",
                    Extension = "jpg",
                    Article = article
                };
                ShumenNewsImage image3 = new ShumenNewsImage
                {
                    Id = "seederImg3",
                    Extension = "jpg",
                    Article = article2
                };
                ShumenNewsImage image4 = new ShumenNewsImage
                {
                    Id = "seederImg4",
                    Extension = "jpg",
                    Article = article2
                };
                ShumenNewsImage image5 = new ShumenNewsImage
                {
                    Id = "seederImg5",
                    Extension = "jpg",
                    Article = article3
                };
                ShumenNewsImage image6 = new ShumenNewsImage
                {
                    Id = "seederImg6",
                    Extension = "jpg",
                    Article = article4
                };
                ShumenNewsImage image7 = new ShumenNewsImage
                {
                    Id = "seederImg7",
                    Extension = "jpg",
                    Article = article4
                };
                ShumenNewsImage image8 = new ShumenNewsImage
                {
                    Id = "seederImg8",
                    Extension = "jpg",
                    Article = article4
                };
                ShumenNewsImage image9 = new ShumenNewsImage
                {
                    Id = "seederImg9",
                    Extension = "jpg",
                    Article = article4
                };
                ShumenNewsImage image10 = new ShumenNewsImage
                {
                    Id = "seederImg10",
                    Extension = "jpg",
                    Article = article4
                };
                ShumenNewsImage image11 = new ShumenNewsImage
                {
                    Id = "seederImg11",
                    Extension = "jpg",
                    Article = article4
                };
                ShumenNewsImage image12 = new ShumenNewsImage
                {
                    Id = "seederImg12",
                    Extension = "jpg",
                    Article = article5
                };
                ShumenNewsImage image13 = new ShumenNewsImage
                {
                    Id = "seederImg13",
                    Extension = "jpg",
                    Article = article6
                };
                db.Images.Add(image);
                db.Images.Add(image2);
                db.Images.Add(image3);
                db.Images.Add(image4);
                db.Images.Add(image5);
                db.Images.Add(image6);
                db.Images.Add(image7);
                db.Images.Add(image8);
                db.Images.Add(image9);
                db.Images.Add(image10);
                db.Images.Add(image11);
                db.Images.Add(image12);
                db.Images.Add(image13);

                //Roles
                db.Roles.Add(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
                db.Roles.Add(new IdentityRole
                {
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                });
                db.Roles.Add(new IdentityRole
                {
                    Name = "Author",
                    NormalizedName = "AUTHOR"
                });
                db.SaveChanges();
            }
        }
    }
}