using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(QuizDbContext db, CancellationToken ct = default)
        {
            // Säkerställer att databasen är uppdaterad
            await db.Database.MigrateAsync(ct);

            // Avbryter om det redan finns data
            if (await db.Categories.AnyAsync(ct))
                return;

            // Viktigt att notera att kategorier + frågor + svar är hämtade från API (kan vara fel)


            // --- KATEGORIER ---
            var categories = new List<Category>
            {
                new() { Name = "Nätverkssäkerhet", Description = "Brandväggar, protokoll och nätverk." },
                new() { Name = "Applikationssäkerhet", Description = "Säker kodning och OWASP." },
                new() { Name = "Kryptering & Kryptografi", Description = "Algoritmer och dataskydd." },
                new() { Name = "Social Engineering", Description = "Mänsklig manipulation." }
            };
            db.Categories.AddRange(categories);
            await db.SaveChangesAsync(ct);

            // --- SUBKATEGORIER (Svårighetsgrader) ---
            var subCategories = new List<SubCategory>();
            foreach (var cat in categories)
            {
                subCategories.Add(new SubCategory { Name = "Basic", CategoryId = cat.Id, Order = 1 });
                subCategories.Add(new SubCategory { Name = "Advanaced", CategoryId = cat.Id, Order = 2 });
                subCategories.Add(new SubCategory { Name = "Expert", CategoryId = cat.Id, Order = 3 });
                subCategories.Add(new SubCategory { Name = "Master", CategoryId = cat.Id, Order = 4 });
            }
            db.SubCategories.AddRange(subCategories);
            await db.SaveChangesAsync(ct);
          
            // Vi använder index från subCategories listan (0-3=Nätverk, 4-7=Applikation, 8-11=Krypto, 12-15=Social)

            // NÄTVERK (Easy - Expert)
            await AddQ(db, subCategories[0].Id, "Vilken port används för okrypterad HTTP?", "80", "443", "22");
            await AddQ(db, subCategories[0].Id, "Vad står VPN för?", "Virtual Private Network", "Verified Public Node", "Variable Port Network");
            await AddQ(db, subCategories[0].Id, "Vilken enhet filtrerar trafik?", "Brandvägg", "Switch", "Hub");

            await AddQ(db, subCategories[1].Id, "Vilket protokoll används för säker fjärrinloggning?", "SSH", "Telnet", "FTP");
            await AddQ(db, subCategories[1].Id, "Vad gör en DHCP-server?", "Delar ut IP-adresser", "Översätter domännamn", "Krypterar trafik");
            await AddQ(db, subCategories[1].Id, "Vilket OSI-lager hanterar IP?", "Lager 3", "Lager 2", "Lager 4");

            await AddQ(db, subCategories[2].Id, "Vad är ett 'SYN Flood'-anfall?", "En typ av DoS-attack", "Lösenordsknäckning", "Nätverks-sniffning");
            await AddQ(db, subCategories[2].Id, "Vilket protokoll använder '3-way handshake'?", "TCP", "UDP", "ICMP");
            await AddQ(db, subCategories[2].Id, "Vad är syftet med ett VLAN?", "Segmentera nätverk logiskt", "Öka hastigheten", "Kryptera diskar");

            await AddQ(db, subCategories[3].Id, "Hur fungerar BGP Hijacking?", "Felaktig annonsering av IP-prefix", "MITM på WiFi", "SQLi i routrar");
            await AddQ(db, subCategories[3].Id, "Vad används Wireshark till?", "Paketanalys", "Lösenordsknäckning", "Fysisk säkerhet");
            await AddQ(db, subCategories[3].Id, "Vilket fält i IP-header minskar vid hopp?", "TTL", "Checksum", "Source IP");

            // APPLIKATION (Easy - Expert)
            await AddQ(db, subCategories[4].Id, "Vilken sårbarhet kör kod i webbläsaren?", "XSS", "SQLi", "Buffer Overflow");
            await AddQ(db, subCategories[4].Id, "Vad står OWASP för?", "Open Web Application Security Project", "Official Web Access", "Online World Safety");
            await AddQ(db, subCategories[4].Id, "Varför starka lösenord?", "Försvåra Brute Force", "Snabbare internet", "Spara utrymme");

            await AddQ(db, subCategories[5].Id, "Vad är syftet med 'Salting'?", "Motverka Rainbow Tables", "Komprimera DB", "Kryptera användarnamn");
            await AddQ(db, subCategories[5].Id, "Bäst mot SQL-injection?", "Parameterized queries", "JS-validering", "Dölja DB-namn");
            await AddQ(db, subCategories[5].Id, "Vad är Broken Access Control?", "Obehörig åtkomst till data", "Sajten ligger nere", "Lösenord utgått");

            await AddQ(db, subCategories[6].Id, "Vad är en CSRF-attack?", "Användaren tvingas utföra handlingar", "Serveröverbelastning", "Kakor stjäls");
            await AddQ(db, subCategories[6].Id, "Header mot iFrame-laddning?", "X-Frame-Options", "Content-Type", "Server-Header");
            await AddQ(db, subCategories[6].Id, "Vad är en Zero-day?", "Okänd sårbarhet för tillverkaren", "Tar 0 dagar att fixa", "Raderar klockan");

            await AddQ(db, subCategories[7].Id, "Vad är en Race Condition?", "Trådar krockar och skapar fel", "Hackertävling", "CPU-överbelastning");
            await AddQ(db, subCategories[7].Id, "Vad är Insecure Deserialization?", "Skadlig kod vid återskapande av objekt", "Okrypterad DB", "Kort API-nyckel");
            await AddQ(db, subCategories[7].Id, "Attack mot XML-processorer?", "XXE", "XSS", "SQLi");

            // KRYPTERING (Easy - Expert)
            await AddQ(db, subCategories[8].Id, "Snabbast kryptering för stora mängder data?", "Symmetrisk", "Asymmetrisk", "Hashing");
            await AddQ(db, subCategories[8].Id, "Process som gör data oläslig?", "Kryptering", "Dekryptering", "Kodning");
            await AddQ(db, subCategories[8].Id, "Vad är en Hash?", "Digitalt fingeravtryck", "Krypterad fil", "Nätverksprotokoll");

            await AddQ(db, subCategories[9].Id, "Vilken algoritm är idag osäker?", "MD5", "AES-256", "SHA-256");
            await AddQ(db, subCategories[9].Id, "Privat nyckel i asymmetrisk används till?", "Dekryptera data", "Kryptera till alla", "Dela med vänner");
            await AddQ(db, subCategories[9].Id, "Vad står AES för?", "Advanced Encryption Standard", "Asymmetric System", "Auto Software");

            await AddQ(db, subCategories[10].Id, "Syftet med Diffie-Hellman?", "Säkert nyckelutbyte", "Lagra lösenord", "Signera e-post");
            await AddQ(db, subCategories[10].Id, "Vad är Perfect Forward Secrecy?", "Gamla sessioner säkra vid läcka", "Oknäckbart", "Byta lösenord ofta");
            await AddQ(db, subCategories[10].Id, "Längd på SHA-256?", "256 bitar", "256 bytes", "128 bitar");

            await AddQ(db, subCategories[11].Id, "Kvantdator-attack mot RSA?", "Shors algoritm", "Snabbare Brute Force", "Social Engineering");
            await AddQ(db, subCategories[11].Id, "Vad är Homomorfisk kryptering?", "Beräkningar på krypterad data", "Självmodifierande", "Mobilkryptering");
            await AddQ(db, subCategories[11].Id, "Kurva i modern krypto?", "Elliptiska kurvor", "Linjära kurvor", "Kvadratiska kurvor");

            // SOCIAL ENGINEERING (Easy - Expert)
            await AddQ(db, subCategories[12].Id, "Leta info i papperskorg?", "Dumpster Diving", "Phishing", "Shoulder Surfing");
            await AddQ(db, subCategories[12].Id, "Målet med SE?", "Manipulera människor", "Hacka servrar direkt", "Snabb kod");
            await AddQ(db, subCategories[12].Id, "Vad är Shoulder Surfing?", "Titta över axeln vid inloggning", "WiFi-hack", "Surfa utan lov");

            await AddQ(db, subCategories[13].Id, "Vad är Pretexting?", "Hitta på en historia för info", "Virus-SMS", "Portskanning");
            await AddQ(db, subCategories[13].Id, "Phishing mot specifik person?", "Spear Phishing", "Whaling", "Vishing");
            await AddQ(db, subCategories[13].Id, "Vad är Tailgating?", "Följa efter genom låst dörr", "Hacka bil", "Sno datortid");

            await AddQ(db, subCategories[14].Id, "Vad är Whaling?", "Phishing mot chefer", "Attack mot stora DB", "Sänka serverhall");
            await AddQ(db, subCategories[14].Id, "Vad är Vishing?", "Voice Phishing", "Virus via phishing", "Video-hack");
            await AddQ(db, subCategories[14].Id, "Vad är Baiting?", "Lämna infekterat USB", "Massepost", "Ringa IT-support");

            await AddQ(db, subCategories[15].Id, "Vad är Quid Pro Quo?", "Tjänst i utbyte mot info", "Hota med polis", "Tekniskt flum");
            await AddQ(db, subCategories[15].Id, "Princip vid 'Scarcity'?", "Brådska (FOMO)", "Lust att hjälpa", "Respekt för auktoritet");
            await AddQ(db, subCategories[15].Id, "Watering Hole attack?", "Infektera sajt målgruppen besöker", "Hacka kaffemaskin", "Stänga av vatten");
        }

        private static async Task AddQ(QuizDbContext db, int subId, string text, string correct, string w1, string w2)
        {
            var q = new Question { Text = text, SubCategoryId = subId };
            db.Questions.Add(q);
            await db.SaveChangesAsync();

            db.AnswerOptions.AddRange(
                new AnswerOption { Text = correct, IsCorrect = true, QuestionId = q.Id },
                new AnswerOption { Text = w1, IsCorrect = false, QuestionId = q.Id },
                new AnswerOption { Text = w2, IsCorrect = false, QuestionId = q.Id }
            );
            await db.SaveChangesAsync();
        }
    }

}
