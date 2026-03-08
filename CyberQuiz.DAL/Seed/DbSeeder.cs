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
                new() { Name = "Network Security", Description = "Firewalls, protocols, and infrastructure protection." },
                new() { Name = "Application Security", Description = "Secure coding, OWASP, and web vulnerabilities." },
                new() { Name = "Cryptography", Description = "Algorithms, encryption, and data integrity." },
                new() { Name = "Social Engineering", Description = "Human manipulation and psychological attacks." }
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
            // NETWORK SECURITY - Basic (subCategories[0])
            await AddQ(db, subCategories[0].Id, "Which port is used for unencrypted HTTP traffic?", "80", "443", "22", "8080");
            await AddQ(db, subCategories[0].Id, "What does VPN stand for?", "Virtual Private Network", "Verified Public Node", "Variable Port Network", "Virtual Protocol Navigation");
            await AddQ(db, subCategories[0].Id, "Which device is primarily used to filter network traffic?", "Firewall", "Switch", "Hub", "Access Point");

            // NETWORK SECURITY - Advanced (subCategories[1])
            await AddQ(db, subCategories[1].Id, "Which protocol is used for secure remote login?", "SSH", "Telnet", "FTP", "RDP");
            await AddQ(db, subCategories[1].Id, "What is the primary function of a DHCP server?", "Assigning IP addresses", "Translating domain names", "Encrypting traffic", "Filtering malware");
            await AddQ(db, subCategories[1].Id, "Which OSI layer handles IP addressing and routing?", "Layer 3", "Layer 2", "Layer 4", "Layer 7");

            // NETWORK SECURITY - Expert (subCategories[2])
            await AddQ(db, subCategories[2].Id, "What is a 'SYN Flood' attack?", "A type of DoS attack", "Password cracking", "Network sniffing", "Database injection");
            await AddQ(db, subCategories[2].Id, "Which protocol uses a '3-way handshake' to establish a connection?", "TCP", "UDP", "ICMP", "IGMP");
            await AddQ(db, subCategories[2].Id, "What is the primary purpose of a VLAN?", "Logical network segmentation", "Increasing physical link speed", "Encrypting local disks", "Redundant power supply");

            // NETWORK SECURITY - Master (subCategories[3])
            await AddQ(db, subCategories[3].Id, "How does BGP Hijacking primarily work?", "Incorrect advertising of IP prefixes", "Man-in-the-Middle on public WiFi", "SQL injection in core routers", "Brute forcing DNS records");
            await AddQ(db, subCategories[3].Id, "What is the primary use of Wireshark?", "Packet analysis", "Password brute forcing", "Physical security auditing", "Creating firewalls");
            await AddQ(db, subCategories[3].Id, "Which field in the IP header is decremented at each hop?", "TTL", "Checksum", "Source IP", "Payload Length");

            // APPLIKATION (Easy - Expert)
            // APPLICATION SECURITY - Basic (subCategories[4])
            await AddQ(db, subCategories[4].Id, "Which vulnerability allows execution of malicious scripts in a user's browser?", "XSS", "SQLi", "Buffer Overflow", "DDoS");
            await AddQ(db, subCategories[4].Id, "What does OWASP stand for?", "Open Web Application Security Project", "Official Web Access Safety", "Online World Systems", "Organized Web App Standards");
            await AddQ(db, subCategories[4].Id, "What is the primary reason for using strong passwords?", "To hinder Brute Force attacks", "To increase internet speed", "To save server storage", "To prevent hardware failure");

            // APPLICATION SECURITY - Advanced (subCategories[5])
            await AddQ(db, subCategories[5].Id, "What is the primary purpose of 'Salting' in password hashing?", "To counter Rainbow Table attacks", "To compress the database", "To encrypt the username", "To speed up the login process");
            await AddQ(db, subCategories[5].Id, "Which is the best defense against SQL Injection?", "Parameterized queries", "JavaScript validation", "Hiding database table names", "Using complex CSS");
            await AddQ(db, subCategories[5].Id, "What characterizes 'Broken Access Control'?", "Unauthorized access to data or functions", "The website being offline", "User passwords being expired", "Slow server response times");

            // APPLICATION SECURITY - Expert (subCategories[6])
            await AddQ(db, subCategories[6].Id, "What happens during a CSRF attack?", "The user is forced to perform unwanted actions", "The server is overloaded with traffic", "The user's cookies are deleted", "The database is completely wiped");
            await AddQ(db, subCategories[6].Id, "Which HTTP header is used to prevent clickjacking by blocking iFrame loading?", "X-Frame-Options", "Content-Type", "Server-Header", "Referrer-Policy");
            await AddQ(db, subCategories[6].Id, "What defines a 'Zero-day' vulnerability?", "A flaw unknown to the software vendor", "A bug that takes 0 days to fix", "An attack that only works at midnight", "A vulnerability that deletes the system clock");

            // APPLICATION SECURITY - Master (subCategories[7])
            await AddQ(db, subCategories[7].Id, "What occurs in a 'Race Condition'?", "Threads compete and cause unexpected errors", "A competition between hackers", "Intentional CPU over-clocking", "Network packets arriving too fast");
            await AddQ(db, subCategories[7].Id, "What is 'Insecure Deserialization'?", "Malicious code execution during object reconstruction", "An unencrypted database connection", "Using a too short API key", "Improper use of global variables");
            await AddQ(db, subCategories[7].Id, "Which attack specifically targets XML processors?", "XXE", "XSS", "SQLi", "CSRF");

            // KRYPTERING (Easy - Expert)
            // CRYPTOGRAPHY - Basic (subCategories[8])
            await AddQ(db, subCategories[8].Id, "Which type of encryption is fastest for large amounts of data?", "Symmetric", "Asymmetric", "Hashing", "Steganography");
            await AddQ(db, subCategories[8].Id, "What is the process of making data unreadable to unauthorized users?", "Encryption", "Decryption", "Encoding", "Compression");
            await AddQ(db, subCategories[8].Id, "What is a cryptographic Hash?", "A digital fingerprint", "An encrypted file", "A network protocol", "A public key");

            // CRYPTOGRAPHY - Advanced (subCategories[9])
            await AddQ(db, subCategories[9].Id, "Which algorithm is currently considered insecure for modern use?", "MD5", "AES-256", "SHA-256", "RSA-4096");
            await AddQ(db, subCategories[9].Id, "In asymmetric encryption, what is the private key used for?", "Decrypting data", "Encrypting for everyone", "Sharing with friends", "Public identification");
            await AddQ(db, subCategories[9].Id, "What does AES stand for?", "Advanced Encryption Standard", "Asymmetric Encryption System", "Auto Encryption Software", "Applied Enterprise Security");

            // CRYPTOGRAPHY - Expert (subCategories[10])
            await AddQ(db, subCategories[10].Id, "What is the primary purpose of the Diffie-Hellman algorithm?", "Secure key exchange", "Storing passwords", "Signing emails", "Formatting database tables");

            await AddQ(db, subCategories[10].Id, "What is the benefit of Perfect Forward Secrecy?", "Past sessions remain secure if a long-term key is leaked", "The system becomes unbreakable", "It forces users to change passwords frequently", "It increases network bandwidth");
            await AddQ(db, subCategories[10].Id, "What is the output length of a SHA-256 hash?", "256 bits", "256 bytes", "128 bits", "512 bits");

            // CRYPTOGRAPHY - Master (subCategories[11])
            await AddQ(db, subCategories[11].Id, "Which quantum computer algorithm poses a major threat to RSA?", "Shor's algorithm", "Grover's algorithm", "Brute Force algorithm", "Social Engineering algorithm");
            await AddQ(db, subCategories[11].Id, "What characterizes Homomorphic Encryption?", "Performing computations on encrypted data", "Self-modifying code", "Mobile-only encryption", "Linear time complexity");
            await AddQ(db, subCategories[11].Id, "Which type of curve is widely used in modern cryptography?", "Elliptic curves", "Linear curves", "Quadratic curves", "Circular curves");


            // SOCIAL ENGINEERING (Easy - Expert)
            // SOCIAL ENGINEERING - Basic (subCategories[12])
            await AddQ(db, subCategories[12].Id, "What is the term for looking for sensitive information in the trash?", "Dumpster Diving", "Phishing", "Shoulder Surfing", "Piggybacking");
            await AddQ(db, subCategories[12].Id, "What is the primary goal of Social Engineering?", "Manipulating people", "Hacking servers directly", "Writing fast code", "Optimizing databases");
            await AddQ(db, subCategories[12].Id, "What does 'Shoulder Surfing' involve?", "Watching someone enter credentials over their shoulder", "Hacking public WiFi", "Browsing without permission", "Interception of radio signals");

            // SOCIAL ENGINEERING - Advanced (subCategories[13])
            await AddQ(db, subCategories[13].Id, "What is 'Pretexting' in social engineering?", "Creating a fabricated scenario to steal info", "Sending a virus via SMS", "Scanning network ports", "Using fake credentials at a gym");
            await AddQ(db, subCategories[13].Id, "What is a phishing attack targeted at a specific individual called?", "Spear Phishing", "Whaling", "Vishing", "Smishing");
            await AddQ(db, subCategories[13].Id, "What is 'Tailgating' in physical security?", "Following an authorized person through a locked door", "Hacking a car's GPS", "Stealing computer time", "Monitoring network traffic from behind");

            // SOCIAL ENGINEERING - Expert (subCategories[14])
            await AddQ(db, subCategories[14].Id, "What is 'Whaling'?", "Phishing targeting high-level executives", "An attack against large databases", "Flooding a server room", "A broad email blast to many users");
            await AddQ(db, subCategories[14].Id, "What does 'Vishing' stand for?", "Voice Phishing", "Virus via phishing", "Video hacking", "Virtual phishing");
            await AddQ(db, subCategories[14].Id, "What is 'Baiting' in social engineering?", "Leaving an infected USB drive for someone to find", "Sending mass emails", "Calling IT support for help", "Buying fake domain names");

            // SOCIAL ENGINEERING - Master (subCategories[15])
            await AddQ(db, subCategories[15].Id, "What is 'Quid Pro Quo' in this context?", "Requesting info in exchange for a service", "Threatening with legal action", "Using technical jargon to confuse", "Pretending to be a CEO");
            await AddQ(db, subCategories[15].Id, "Which principle is used in 'Scarcity' attacks?", "Urgency (FOMO)", "The desire to help", "Respect for authority", "Social proof");
            await AddQ(db, subCategories[15].Id, "What is a 'Watering Hole' attack?", "Infecting a website the target group visits often", "Hacking the office coffee machine", "Disrupting the building's water supply", "DDoS attack on a local server");
        }

        private static async Task AddQ(QuizDbContext db, int subId, string text, string correct, string w1, string w2, string w3)
        {
            var q = new Question { Text = text, SubCategoryId = subId };
            db.Questions.Add(q);
            await db.SaveChangesAsync();

            db.AnswerOptions.AddRange(
                new AnswerOption { Text = correct, IsCorrect = true, QuestionId = q.Id },
                new AnswerOption { Text = w1, IsCorrect = false, QuestionId = q.Id },
                new AnswerOption { Text = w2, IsCorrect = false, QuestionId = q.Id },
                new AnswerOption { Text = w3, IsCorrect = false, QuestionId = q.Id }
            );
            await db.SaveChangesAsync();
        }
    }

}
