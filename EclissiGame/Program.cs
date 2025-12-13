using System;

class Program
{
    // ---------- FUNZIONI ----------

    // Mostra le istruzioni iniziali
    static void MostraIstruzioni()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("        ECLISSI GALATTICA: L'ULTIMO RANGER");
        Console.WriteLine("==============================================");
        Console.WriteLine("- Scegli il tuo personaggio: A-47 (+5 vita), Kael (+1 danno), Nyra (+1 fuga)");
        Console.WriteLine("- Avanza sulla mappa di 20 caselle, ogni casella ha eventi o bonus");
        Console.WriteLine("- Usa oggetti prima dei combattimenti");
        Console.WriteLine("- Obiettivo: arrivare all'ARK-01 senza morire!");
        Console.WriteLine("Oggetti possibili:");
        Console.WriteLine();
        Console.WriteLine("• Nanokit Medico (+10 vita)");
        Console.WriteLine("• Stim di Rigenerazione (+20 vita)");
        Console.WriteLine("• Pistola a Ioni (+1 danno)");
        Console.WriteLine("• Spada Magnetica (+2 danno)");
        Console.WriteLine("• Motore Sub-Luce Danneggiato (cavalcatura +1 casella)");
        Console.WriteLine("==============================================");
        Console.WriteLine("Premi INVIO per iniziare...");
        Console.ReadLine();
    }

    // Mostra lo status del giocatore
    static void MostraStatus(int numeroCasella, int puntiVita, int danno, int probabilitaFuga, string[] zaino)
    {
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Casella attuale: {numeroCasella}/20");
        Console.WriteLine($"Vita: {puntiVita} | Danno: {danno} | Probabilità fuga: {probabilitaFuga}");
        Console.WriteLine("Zaino:");

        bool zainoVuoto = true;
        for (int i = 0; i < zaino.Length; i++)
        {
            if (zaino[i] == null || zaino[i] == "")
            {
                Console.WriteLine(" - " + zaino[i]);
                zainoVuoto = false;
            }

        }
        if (zainoVuoto)
        {
            Console.WriteLine(" (vuoto)");

        }

        Console.WriteLine("----------------------------------------");
    }

    // Trova un oggetto casuale
    static string TrovaOggettoCasuale(Random rnd)
    {
        string[] oggetti = { "Nanokit Medico", "Stim di Rigenerazione", "Pistola a Ioni", "Spada Magnetica", "Motore Sub-Luce Danneggiato" };
        return oggetti[rnd.Next(oggetti.Length)];
    }

    // Aggiunge oggetto nello zaino 
    static void AggiungiOggettoNelloZaino(string[] zaino, string oggetto)
    {
        for (int i = 0; i < zaino.Length; i++)
        {
            if (string.IsNullOrEmpty(zaino[i]))
            {
                zaino[i] = oggetto;
                Console.WriteLine($"Hai aggiunto {oggetto} allo zaino!");
                return;
            }
        }
        Console.WriteLine("Lo zaino è pieno! Non puoi raccogliere questo oggetto.");
    }

    // Usa un oggetto
    static void UsaOggetto(string oggetto, ref int puntiVita, ref bool cavalcatura, ref int danno)
    {
        if (oggetto == "Nanokit Medico")
        {

            Console.WriteLine("+10 vita!"); puntiVita += 10;

        }
        else if (oggetto == "Stim di Rigenerazione")
        {

            Console.WriteLine("+20 vita!"); puntiVita += 20;

        }
        else if (oggetto == "Pistola a Ioni")
        {

            Console.WriteLine("+1 danno!"); danno += 1;

        }
        else if (oggetto == "Spada Magnetica")
        {
            Console.WriteLine("+2 danno!"); danno += 2;

        }
        else if (oggetto == "Motore Sub-Luce Danneggiato")
        {
            Console.WriteLine("Ora hai una cavalcatura! +1 casella");
            cavalcatura = true;

        }
        else Console.WriteLine("Oggetto non utilizzabile ora.");
    }

    // Rimuove oggetto dallo zaino
    static void RimuoviOggettoDalloZaino(string[] zaino, string oggetto)
    {
        for (int i = 0; i < zaino.Length; i++)
        {
            if (zaino[i] == oggetto)
            {
                zaino[i] = "";
                return;
            }
        }
    }

    // Combattimento
    static void Combattimento(ref int puntiVitaGiocatore, int dannoGiocatore, Random rnd, string nemico, int vitaNemico, int dannoNemico, int probabilitaFuga)
    {
        Console.WriteLine($"\nCombattimento contro: {nemico} | Vita nemico: {vitaNemico} | Danno: {dannoNemico}");

        while (puntiVitaGiocatore > 0 && vitaNemico > 0)
        {
            int fortuna = rnd.Next(1, 7);

            if (fortuna == 1)
            {
                Console.WriteLine("Sei fortunato! Il nemico manca l'attacco.");
            }
            else
            {
                int danno = rnd.Next(1, dannoGiocatore + 1);
                vitaNemico -= danno;
                Console.WriteLine($"Infliggi {danno} danni!");

                if (vitaNemico > 0)
                {
                    int dannoSubito = rnd.Next(1, dannoNemico + 1);
                    puntiVitaGiocatore -= dannoSubito;
                    Console.WriteLine($"Subisci {dannoSubito} danni!");
                }
            }
        }

        if (puntiVitaGiocatore <= 0) Console.WriteLine("Sei stato sconfitto...");
        else Console.WriteLine("Hai sconfitto il nemico!");
    }

    // Evento casella
    static void EventoCasella(ref int puntiVita, ref int danno, ref int probabilitaFuga, ref bool cavalcatura, string[] zaino, Random rnd, string descrizioneCasella)
    {
        Console.WriteLine($"\n{descrizioneCasella}");
        int tipoEvento = rnd.Next(0, 3); // 0=combattimento, 1=oggetto, 2=personaggio

        if (tipoEvento == 0)
        {
            Console.WriteLine("Hai incontrato un nemico!");

            for (int i = 0; i < zaino.Length; i++)
            {
                if (!string.IsNullOrEmpty(zaino[i]))
                {
                    Console.WriteLine($"Vuoi usare {zaino[i]} prima del combattimento? (s/n)");
                    string risposta = Console.ReadLine();
                    if (risposta == "s")
                    {
                        UsaOggetto(zaino[i], ref puntiVita, ref cavalcatura, ref danno);
                        zaino[i] = "";
                    }
                }
            }

            int vitaNemico = rnd.Next(5, 11);
            int dannoNemico = rnd.Next(1, 4);
            Combattimento(ref puntiVita, danno, rnd, "Ombroforme Minore", vitaNemico, dannoNemico, probabilitaFuga);
        }
        else if (tipoEvento == 1)
        {
            string oggetto = TrovaOggettoCasuale(rnd);
            Console.WriteLine($"Hai trovato: {oggetto}. Vuoi raccoglierlo? (s/n)");
            string risposta = Console.ReadLine();
            if (risposta == "s") AggiungiOggettoNelloZaino(zaino, oggetto);
        }
        else
        {
            int personaggio = rnd.Next(0, 3);

            if (personaggio == 0)
            {
                Console.WriteLine("Incontri un Ranger sopravvissuto che ti offre un bonus!");
                Console.WriteLine("1) Accetta +10 vita  2) Ignora");
                string scelta = Console.ReadLine();
                if (scelta == "1") puntiVita += 10;
            }
            else if (personaggio == 1)
            {
                Console.WriteLine("Un mercante interstellare ti offre una scelta:");
                Console.WriteLine("1) Aggiungi una Pistola a Ioni  2) Niente");
                string scelta = Console.ReadLine();
                if (scelta == "1") AggiungiOggettoNelloZaino(zaino, "Pistola a Ioni");
            }
            else
            {
                Console.WriteLine("Un impostore ti attacca!");
                int vitaNemico = rnd.Next(5, 8);
                int dannoNemico = rnd.Next(1, 3);
                Combattimento(ref puntiVita, danno, rnd, "Impostore", vitaNemico, dannoNemico, probabilitaFuga);
            }
        }
    }

    // Dado movimento 1-4
    static int TiraDado(Random rnd) => rnd.Next(1, 5);

    static void AspettaInput()
    {
        Console.WriteLine("\nPremi INVIO per continuare...");
        Console.ReadLine();
    }

    // ---------------- MAIN ----------------
    static void Main()
    {
        MostraIstruzioni();
        Random rnd = new Random();

        // Scelta personaggio
        Console.WriteLine("Scegli il tuo personaggio: 1) A-47  2) Kael  3) Nyra");
        string scelta = Console.ReadLine();

        int puntiVita = 20, danno = 2, probabilitaFuga = 1;
        bool cavalcatura = false;

        if (scelta == "1") puntiVita += 5;
        else if (scelta == "2") danno += 1;
        else if (scelta == "3") probabilitaFuga += 1;

        // --- ZAINO CORRETTO ---
        string[] zaino = new string[10];
        for (int i = 0; i < zaino.Length; i++)
            zaino[i] = "";

        int posizione = 0;

        // Mappa
        string[] mappa = {
            "Detriti Orbitali: navi distrutte galleggiano...",
            "Stiva della Falcon-03: rumori metallici...",
            "Tunnel di Manutenzione: luci rosse lampeggiano...",
            "Settore RAD-2 (radiazioni)...",
            "Laboratorio Abbandonato...",
            "Corridoi Meccanici...",
            "Serbatoi Criogenici...",
            "Hangar Fantasma...",
            "Osservatorio Stellare...",
            "Giardino Idroponico...",
            "Stanza degli Ologrammi...",
            "Archivio Quantico...",
            "Cunicolo di Ventilazione...",
            "Nodo Energetico Centrale...",
            "Condotti di Rifiuti...",
            "Laboratorio di Xenobiologia...",
            "Ponte di Comando...",
            "Cubo di Stabilizzazione...",
            "Corridoio dell’Eclissi...",
            "ARK-01 (finale): il destino della galassia..."
        };

        // Loop di gioco
        while (puntiVita > 0 && posizione < 19)
        {
            Console.Clear();
            Console.WriteLine($"Sei in: {mappa[posizione]}");
            MostraStatus(posizione + 1, puntiVita, danno, probabilitaFuga, zaino);

            Console.WriteLine("Premi INVIO per tirare il dado...");
            Console.ReadLine();

            int tiro = TiraDado(rnd);
            int avanzamento = tiro;

            if (cavalcatura)
            {
                avanzamento += 1;
                Console.WriteLine("La cavalcatura ti fa avanzare di 1 casella extra!");
            }

            posizione += avanzamento;
            if (posizione > 19) posizione = 19;

            Console.WriteLine($"Hai tirato {tiro}. Avanzi di {avanzamento} caselle.");
            Console.WriteLine($"Ora sei in: {mappa[posizione]}");

            EventoCasella(ref puntiVita, ref danno, ref probabilitaFuga, ref cavalcatura, zaino, rnd, mappa[posizione]);

            if (puntiVita <= 0)
                Console.WriteLine("Sei morto... Game Over.");

            AspettaInput();
        }

        if (puntiVita > 0 && posizione >= 19)
        {
            Console.WriteLine(" Sei arrivato all'ARK-01! Missione completata!");
        }
    }
}


