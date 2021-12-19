using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public class MockDataHelper : IMockDataHelper
    {
        private static Random random = new Random();

        List<string> surnameList = new List<string>();
        List<string> nameList = new List<string>();

        public MockDataHelper()
        {
            LoadSurnames();
            LoadNames();
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string GenerateSurname()
        {
            string str = string.Empty;
            str = surnameList.OrderBy(xx => random.Next()).First();
            return str;
        }
        public string GenerateName()
        {

            string str = string.Empty;

            str = nameList.OrderBy(xx => random.Next()).First();
            return str;
        }


        private void LoadSurnames()
        {
            
            surnameList.Add("Smith");
            surnameList.Add("Johnson");
            surnameList.Add("Williams");
            surnameList.Add("Jones");
            surnameList.Add("Brown");
            surnameList.Add("Davis");
            surnameList.Add("Miller");
            surnameList.Add("Wilson");
            surnameList.Add("Moore");
            surnameList.Add("Taylor");
            surnameList.Add("Anderson");
            surnameList.Add("Thomas");
            surnameList.Add("Jackson");
            surnameList.Add("White");
            surnameList.Add("Harris");
            surnameList.Add("Martin");
            surnameList.Add("Thompson");
            surnameList.Add("Garcia");
            surnameList.Add("Martinez");
            surnameList.Add("Robinson");
            surnameList.Add("Clark");
            surnameList.Add("Rodriguez");
            surnameList.Add("Lewis");
            surnameList.Add("Lee");
            surnameList.Add("Walker");
            surnameList.Add("Hall");
            surnameList.Add("Allen");
            surnameList.Add("Young");
            surnameList.Add("Hernandez");
            surnameList.Add("King");
            surnameList.Add("Wright");
            surnameList.Add("Lopez");
            surnameList.Add("Hill");
            surnameList.Add("Scott");
            surnameList.Add("Green");
            surnameList.Add("Adams");
            surnameList.Add("Baker");
            surnameList.Add("Gonzalez");
            surnameList.Add("Nelson");
            surnameList.Add("Carter");
            surnameList.Add("Mitchell");
            surnameList.Add("Perez");
            surnameList.Add("Roberts");
            surnameList.Add("Turner");
            surnameList.Add("Phillips");
            surnameList.Add("Campbell");
            surnameList.Add("Parker");
            surnameList.Add("Evans");
            surnameList.Add("Edwards");
            surnameList.Add("Collins");
            surnameList.Add("Stewart");
            surnameList.Add("Sanchez");
            surnameList.Add("Morris");
            surnameList.Add("Rogers");
            surnameList.Add("Reed");
            surnameList.Add("Cook");
            surnameList.Add("Morgan");
            surnameList.Add("Bell");
            surnameList.Add("Murphy");
            surnameList.Add("Bailey");
            surnameList.Add("Rivera");
            surnameList.Add("Cooper");
            surnameList.Add("Richardson");
            surnameList.Add("Cox");
            surnameList.Add("Howard");
            surnameList.Add("Ward");
            surnameList.Add("Torres");
            surnameList.Add("Peterson");
            surnameList.Add("Gray");
            surnameList.Add("Ramirez");
            surnameList.Add("James");
            surnameList.Add("Watson");
            surnameList.Add("Brooks");
            surnameList.Add("Kelly");
            surnameList.Add("Sanders");
            surnameList.Add("Price");
            surnameList.Add("Bennett");
            surnameList.Add("Wood");
            surnameList.Add("Barnes");
            surnameList.Add("Ross");
            surnameList.Add("Henderson");
            surnameList.Add("Coleman");
            surnameList.Add("Jenkins");
            surnameList.Add("Perry");
            surnameList.Add("Powell");
            surnameList.Add("Long");
            surnameList.Add("Patterson");
            surnameList.Add("Hughes");
            surnameList.Add("Flores");
            surnameList.Add("Washington");
            surnameList.Add("Butler");
            surnameList.Add("Simmons");
            surnameList.Add("Foster");
            surnameList.Add("Gonzales");
            surnameList.Add("Bryant");
            surnameList.Add("Alexander");
            surnameList.Add("Russell");
            surnameList.Add("Griffin");
            surnameList.Add("Diaz");
            surnameList.Add("Hayes");
        }
        private void LoadNames()
        {
            nameList.Add("Aiden");
            nameList.Add("Jackson");
            nameList.Add("Mason");
            nameList.Add("Liam");
            nameList.Add("Jacob");
            nameList.Add("Jayden");
            nameList.Add("Ethan");
            nameList.Add("Noah");
            nameList.Add("Lucas");
            nameList.Add("Logan");
            nameList.Add("Caleb");
            nameList.Add("Caden");
            nameList.Add("Jack");
            nameList.Add("Ryan");
            nameList.Add("Connor");
            nameList.Add("Michael");
            nameList.Add("Elijah");
            nameList.Add("Brayden");
            nameList.Add("Benjamin");
            nameList.Add("Nicholas");
            nameList.Add("Alexander");
            nameList.Add("William");
            nameList.Add("Matthew");
            nameList.Add("James");
            nameList.Add("Landon");
            nameList.Add("Nathan");
            nameList.Add("Dylan");
            nameList.Add("Evan");
            nameList.Add("Luke");
            nameList.Add("Andrew");
            nameList.Add("Gabriel");
            nameList.Add("Gavin");
            nameList.Add("Joshua");
            nameList.Add("Owen");
            nameList.Add("Daniel");
            nameList.Add("Carter");
            nameList.Add("Tyler");
            nameList.Add("Cameron");
            nameList.Add("Christian");
            nameList.Add("Wyatt");
            nameList.Add("Henry");
            nameList.Add("Eli");
            nameList.Add("Joseph");
            nameList.Add("Max");
            nameList.Add("Isaac");
            nameList.Add("Samuel");
            nameList.Add("Anthony");
            nameList.Add("Grayson");
            nameList.Add("Zachary");
            nameList.Add("David");
            nameList.Add("Christopher");
            nameList.Add("John");
            nameList.Add("Isaiah");
            nameList.Add("Levi");
            nameList.Add("Jonathan");
            nameList.Add("Oliver");
            nameList.Add("Chase");
            nameList.Add("Cooper");
            nameList.Add("Tristan");
            nameList.Add("Colton");
            nameList.Add("Austin");
            nameList.Add("Colin");
            nameList.Add("Charlie");
            nameList.Add("Dominic");
            nameList.Add("Parker");
            nameList.Add("Hunter");
            nameList.Add("Thomas");
            nameList.Add("Alex");
            nameList.Add("Ian");
            nameList.Add("Jordan");
            nameList.Add("Cole");
            nameList.Add("Julian");
            nameList.Add("Aaron");
            nameList.Add("Carson");
            nameList.Add("Miles");
            nameList.Add("Blake");
            nameList.Add("Brody");
            nameList.Add("Adam");
            nameList.Add("Sebastian");
            nameList.Add("Adrian");
            nameList.Add("Nolan");
            nameList.Add("Sean");
            nameList.Add("Riley");
            nameList.Add("Bentley");
            nameList.Add("Xavier");
            nameList.Add("Hayden");
            nameList.Add("Jeremiah");
            nameList.Add("Jason");
            nameList.Add("Jake");
            nameList.Add("Asher");
            nameList.Add("Micah");
            nameList.Add("Jace");
            nameList.Add("Brandon");
            nameList.Add("Josiah");
            nameList.Add("Hudson");
            nameList.Add("Nathaniel");
            nameList.Add("Bryson");
            nameList.Add("Ryder");
            nameList.Add("Justin");
            nameList.Add("Bryce");
            //—————female
            nameList.Add("Sophia");
            nameList.Add("Emma");
            nameList.Add("Isabella");
            nameList.Add("Olivia");
            nameList.Add("Ava");
            nameList.Add("Lily");
            nameList.Add("Chloe");
            nameList.Add("Madison");
            nameList.Add("Emily");
            nameList.Add("Abigail");
            nameList.Add("Addison");
            nameList.Add("Mia");
            nameList.Add("Madelyn");
            nameList.Add("Ella");
            nameList.Add("Hailey");
            nameList.Add("Kaylee");
            nameList.Add("Avery");
            nameList.Add("Kaitlyn");
            nameList.Add("Riley");
            nameList.Add("Aubrey");
            nameList.Add("Brooklyn");
            nameList.Add("Peyton");
            nameList.Add("Layla");
            nameList.Add("Hannah");
            nameList.Add("Charlotte");
            nameList.Add("Bella");
            nameList.Add("Natalie");
            nameList.Add("Sarah");
            nameList.Add("Grace");
            nameList.Add("Amelia");
            nameList.Add("Kylie");
            nameList.Add("Arianna");
            nameList.Add("Anna");
            nameList.Add("Elizabeth");
            nameList.Add("Sophie");
            nameList.Add("Claire");
            nameList.Add("Lila");
            nameList.Add("Aaliyah");
            nameList.Add("Gabriella");
            nameList.Add("Elise");
            nameList.Add("Lillian");
            nameList.Add("Samantha");
            nameList.Add("Makayla");
            nameList.Add("Audrey");
            nameList.Add("Alyssa");
            nameList.Add("Ellie");
            nameList.Add("Alexis");
            nameList.Add("Isabelle");
            nameList.Add("Savannah");
            nameList.Add("Evelyn");
            nameList.Add("Leah");
            nameList.Add("Keira");
            nameList.Add("Allison");
            nameList.Add("Maya");
            nameList.Add("Lucy");
            nameList.Add("Sydney");
            nameList.Add("Taylor");
            nameList.Add("Molly");
            nameList.Add("Lauren");
            nameList.Add("Harper");
            nameList.Add("Scarlett");
            nameList.Add("Brianna");
            nameList.Add("Victoria");
            nameList.Add("Liliana");
            nameList.Add("Aria");
            nameList.Add("Kayla");
            nameList.Add("Annabelle");
            nameList.Add("Gianna");
            nameList.Add("Kennedy");
            nameList.Add("Stella");
            nameList.Add("Reagan");
            nameList.Add("Julia");
            nameList.Add("Bailey");
            nameList.Add("Alexandra");
            nameList.Add("Jordyn");
            nameList.Add("Nora");
            nameList.Add("Carolin");
            nameList.Add("Mackenzie");
            nameList.Add("Jasmine");
            nameList.Add("Jocelyn");
            nameList.Add("Kendall");
            nameList.Add("Morgan");
            nameList.Add("Nevaeh");
            nameList.Add("Maria");
            nameList.Add("Eva");
            nameList.Add("Juliana");
            nameList.Add("Abby");
            nameList.Add("Alexa");
            nameList.Add("Summer");
            nameList.Add("Brooke");
            nameList.Add("Penelope");
            nameList.Add("Violet");
            nameList.Add("Kate");
            nameList.Add("Hadley");
            nameList.Add("Ashlyn");
            nameList.Add("Sadie");
            nameList.Add("Paige");
            nameList.Add("Katherine");
            nameList.Add("Sienna");
            nameList.Add("Piper");

        }

        public int GenerateRandomRate(int end)
        {
            return random.Next(end);
        }
    }
}
