using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public List<Card> player_deck = new List<Card>();
    public List<Card> ai_deck = new List<Card>();
    public List<Card> player_hand = new List<Card>();
    public List<Card> ai_hand = new List<Card>();
    public List<Card> cardPrefabs = new List<Card>();
    public string deckFolder = "Decks";
    public bool useDeckFiles = true;
    public List<Card> card_database = new List<Card>();
    public List<Enemy> enemies = new List<Enemy>();
    public List<Librarian> librarians = new List<Librarian>();
    public List<GameObject> clashers = new List<GameObject>();
    public List<Card> discard_pile = new List<Card>();
    public int initial_hand_size = 4;
    public SpeedDie selected_die;
    public EnemySpeedDie selected_enemy_die;
    public Card selected_card;
    public Librarian selectedl;
    public Enemy selectede;
    public int turn = 0;
    public float clashKnockbackDistance = 100f;
    public float clashKnockbackDuration = 0.08f;
    public float clashKnockbackDamageMultiplier = 2f;
    public float clashWinnerAdvanceDistance = 50f;
    public float clashWinnerAdvanceDuration = 0.08f;


    private void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Debug.Log("Game Manager Started");
        //find all enemies on screen
        enemies = new List<Enemy>(FindObjectsByType<Enemy>());
        librarians = new List<Librarian>(FindObjectsByType<Librarian>());
        turn = 0;
        //wait a frame and then start combat
        yield return new WaitForSeconds(0.1f);
        LoadDeckFiles();
        startcombat();
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }

    void LoadDeckFiles()
    {
        if (!useDeckFiles)
            return;

        string deckPath = Path.Combine(Application.streamingAssetsPath, deckFolder);

        foreach (Librarian librarian in librarians)
        {
            if (librarian == null)
                continue;

            string fileName = string.IsNullOrWhiteSpace(librarian.deckFileName)
                ? librarian.gameObject.name
                : librarian.deckFileName;
            string playerPath = Path.Combine(deckPath, fileName + ".txt");
            List<Card> playerDeck = LoadDeckFromFile(playerPath);

            if (playerDeck != null && playerDeck.Count > 0)
            {
                librarian.truedeck = playerDeck;
                Debug.Log($"Loaded deck for librarian '{librarian.gameObject.name}' from {playerPath} ({playerDeck.Count} cards)");
            }
            else
            {
                Debug.LogWarning($"Librarian deck file not loaded or empty: {playerPath}");
            }
        }

        foreach (Enemy enemy in enemies)
        {
            if (enemy == null)
                continue;

            string fileName = string.IsNullOrWhiteSpace(enemy.deckFileName)
                ? enemy.gameObject.name
                : enemy.deckFileName;
            string enemyPath = Path.Combine(deckPath, fileName + ".txt");
            List<Card> enemyDeck = LoadDeckFromFile(enemyPath);

            if (enemyDeck != null && enemyDeck.Count > 0)
            {
                enemy.truedeck = enemyDeck;
                Debug.Log($"Loaded deck for enemy '{enemy.gameObject.name}' from {enemyPath} ({enemyDeck.Count} cards)");
            }
            else
            {
                Debug.LogWarning($"Enemy deck file not loaded or empty: {enemyPath}");
            }
        }
    }

    List<Card> LoadDeckFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Deck file not found: {filePath}");
            return null;
        }

        string[] lines = File.ReadAllLines(filePath);
        List<Card> loadedDeck = new List<Card>();

        for (int i = 0; i < lines.Length; i++)
        {
            string rawLine = lines[i].Trim();
            if (string.IsNullOrEmpty(rawLine) || rawLine.StartsWith("#"))
                continue;

            Card prefab = FindCardPrefabByName(rawLine);
            if (prefab == null)
            {
                Debug.LogWarning($"Deck file line {i + 1}: card not found: '{rawLine}'. Available cards: {string.Join(", ", cardPrefabs.Where(c => c != null && c.data != null).Select(c => c.data.card_name))}");
                continue;
            }

            loadedDeck.Add(prefab);
        }

        return loadedDeck;
    }

    Card FindCardPrefabByName(string cardName)
    {
        if (cardPrefabs == null)
            return null;

        return cardPrefabs.Find(prefab => prefab != null && prefab.data != null &&
            string.Equals(prefab.data.card_name, cardName, StringComparison.OrdinalIgnoreCase));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Deal(int damount)
    {
        Shuffle();
        for (int i = 0; i < initial_hand_size; i++)
        {
            player_hand.Add(player_deck[0]);
            player_deck.RemoveAt(0);
            ai_hand.Add(ai_deck[0]);
            ai_deck.RemoveAt(0);
        }
    }

    void Shuffle()
    {

    }

    void AI_Turn()
    {

    }

    public void CountCards()
    {
        Debug.Log("counted cards");
        for (int i = 0; i < card_database.Count; i++)
        {
           card_database[i].locationinhand = i;
        }
        
    }

    IEnumerator Knockback(Transform loser, Vector3 sourcePosition, float distance, int damage = 0)
    {
        if (loser == null)
        {
            yield break;
        }

        Vector3 direction = loser.position - sourcePosition;
        if (direction == Vector3.zero)
        {
            direction = Vector3.Scale(Vector3.back, new Vector3(1, 0.5f, 1));
        }
        direction.Normalize();

        // Scale knockback distance based on damage
        float scaledDistance = distance + (damage * clashKnockbackDamageMultiplier);

        Vector3 start = loser.position;
        Vector3 target = start + direction * scaledDistance;
        float elapsed = 0f;

        while (elapsed < clashKnockbackDuration)
        {
            loser.position = Vector3.Lerp(start, target, elapsed / clashKnockbackDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        loser.position = target;
    }

    IEnumerator AdvanceTowards(Transform winner, Vector3 targetPosition, float distance)
    {
        if (winner == null)
        {
            yield break;
        }

        Vector3 direction = targetPosition - winner.position;
        if (direction == Vector3.zero)
        {
            yield break;
        }
        direction.Normalize();

        Vector3 start = winner.position;
        Vector3 target = start + direction * distance;
        float elapsed = 0f;

        while (elapsed < clashWinnerAdvanceDuration)
        {
            winner.position = Vector3.Lerp(start, target, elapsed / clashWinnerAdvanceDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        winner.position = target;
    }

    public void startcombat()
    {
        enemies = new List<Enemy>(FindObjectsByType<Enemy>());
        librarians = new List<Librarian>(FindObjectsByType<Librarian>());
        if (librarians.Count <= 0)
        {
            Debug.Log("Combat cannot start without both librarians and enemies.");
            return;
        }
        
        else if (enemies.Count <= 0)
        {
            //
        }
        

        else
        {
            for (int i = 0; i < librarians.Count; i++)
            {
                librarians[i].transform.position = librarians[i].setlocation;
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].transform.position = enemies[i].setlocation;
            }

            if (turn == 0)
            {
                for (int i = 0; i < librarians.Count; i++)
                {
                    librarians[i].hand.Clear();
                    if (librarians[i].deck.Count == 0 && librarians[i].truedeck.Count > 0)
                    {
                        librarians[i].deck = new List<Card>(librarians[i].truedeck);
                    }
                    librarians[i].ShuffleDeck();
                    librarians[i].draw(4);
                    librarians[i].turnstart();
                    librarians[i].cost = librarians[i].maxcost;
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].hand.Clear();
                    if (enemies[i].deck.Count == 0 && enemies[i].truedeck.Count > 0)
                    {
                        enemies[i].deck = new List<Card>(enemies[i].truedeck);
                    }
                    enemies[i].ShuffleDeck();
                    enemies[i].draw(4);
                    enemies[i].turnstart();
                }
            }       
            else
            {
                for (int i = 0; i < librarians.Count; i++)
                {
                    librarians[i].KillCards();
                    librarians[i].draw(1);
                    librarians[i].turnstart();
                    librarians[i].cost += 1;
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].KillCards();
                    enemies[i].draw(1);
                    enemies[i].turnstart();
                }
            }
        }
    }

    public IEnumerator Clash()
    {
        //figure
        clashers.Clear();
        enemies = new List<Enemy>(FindObjectsByType<Enemy>());
        librarians = new List<Librarian>(FindObjectsByType<Librarian>());
        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = 0; j < enemies[i].dice.Count; j++)
            {
                if (enemies[i].dice[j].clash_target != null)
                {
                    clashers.Add(enemies[i].dice[j].gameObject);
                }
            }
        }

        for (int i = 0; i < librarians.Count; i++)
        {
            for (int j = 0; j < librarians[i].dice.Count; j++)
            {
                if (librarians[i].dice[j].clash_target != null)
                {
                    clashers.Add(librarians[i].dice[j].gameObject);
                }
            }
        }
        
        for (int i = 0; i < clashers.Count; i++)
        {
            //move clasher and its parent towards the clash target
            var speedDie = clashers[i].GetComponent<SpeedDie>();
            var enemySpeedDie = clashers[i].GetComponent<EnemySpeedDie>();
            if ((speedDie != null && speedDie.clashed == false) || (enemySpeedDie != null && enemySpeedDie.clashed == false))
            {
                float moveSpeed = 200f;
                float stopDistance = 100f;
                Vector3 targetPosition;
                Transform parentTransform;
                Vector3 dieWorldOffset;

                if (speedDie != null)
                {
                    targetPosition = speedDie.clash_target.transform.position;
                    parentTransform = speedDie.librarian.transform;
                    clashers[i].GetComponent<SpeedDie>().GetComponentInParent<Librarian>().spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().librarian.move;
                }
                else
                {
                    targetPosition = enemySpeedDie.clash_target.transform.position;
                    parentTransform = enemySpeedDie.GetComponentInParent<Enemy>().transform;
                    clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().move;
                }

                dieWorldOffset = clashers[i].transform.position - parentTransform.position;
                Vector3 directionToTarget = targetPosition - clashers[i].transform.position;
                if (directionToTarget == Vector3.zero)
                {
                    directionToTarget = Vector3.forward;
                }
                directionToTarget = directionToTarget.normalized;
                Vector3 dieTargetPosition = targetPosition - directionToTarget * stopDistance;
                Vector3 parentTargetPosition = dieTargetPosition - dieWorldOffset;

                while (Vector3.Distance(clashers[i].transform.position, dieTargetPosition) > 0.05f)
                {
                    parentTransform.position = Vector3.MoveTowards(parentTransform.position, parentTargetPosition, Time.deltaTime * moveSpeed);
                    yield return null;
                }

                parentTransform.position = parentTargetPosition;

                //clash librarians
                if (clashers[i].GetComponentInParent<Librarian>() != null)
                {
                    
                     
                    // per clasher
                    
                    //per card played
                    selectedl = clashers[i].GetComponentInParent<Librarian>();
                    selectede = clashers[i].GetComponent<SpeedDie>().clash_target.GetComponentInParent<Enemy>();
                    SpeedDie playerDie = clashers[i].GetComponent<SpeedDie>();
                    EnemySpeedDie enemyDie = playerDie.clash_target;
                    int playerDiceCount = playerDie.selected_card.data.dice.Length;
                    int enemyDiceCount = enemyDie.selected_card.data.dice.Length;
                    int maxDice = Mathf.Max(playerDiceCount, enemyDiceCount);
                    for  (int k = 0; k < maxDice; k++)
                    {
                        //per die on card
                        int temp1 = 0;
                        if (k < playerDiceCount)
                        {
                            temp1 = UnityEngine.Random.Range(playerDie.selected_card.data.dice[k].min, playerDie.selected_card.data.dice[k].max);
                        }
                        int temp2 = 0;
                        if (k < enemyDiceCount && enemyDie.clash_target == playerDie)
                        {
                            temp2 = UnityEngine.Random.Range(enemyDie.selected_card.data.dice[k].min, enemyDie.selected_card.data.dice[k].max);
                        }
                        
                        selectedl.UpdateDI(temp1.ToString());
                        selectede.UpdateDI(temp2.ToString());
                        yield return new WaitForSeconds(1);
                        selectedl.UpdateDI("");
                        selectede.UpdateDI("");

                        
                        
                            if (clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].type == "blunt")
                            {
                                clashers[i].GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().librarian.blunt;
                            }
                            else if (clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].type == "pierce")
                            {
                                clashers[i].GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().librarian.pierce;
                            }
                            else if (clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].type == "slash")
                            {
                                clashers[i].GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().librarian.slash;
                            }
                            else if (clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].type == "guard")
                            {
                                clashers[i].GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().librarian.guard;
                            }

                            // If the winner's die is an evade, it deals no damage and the attacker shows "Missed"
                            if (playerDie.selected_card != null && playerDie.selected_card.data.dice[k].type == "evade")
                            {
                                selectedl.UpdateDI("Missed");
                            }
                            else if (k < enemyDiceCount && enemyDie.selected_card.data.dice[k].type == "block")
                            {
                                selectede.health -= temp1 - temp2;
                                selectedl.UpdateDI((temp1 - temp2).ToString());
                            }
                            else
                            {
                                selectede.health -= temp1;
                                selectedl.UpdateDI(temp1.ToString());
                            }

                            yield return StartCoroutine(Knockback(selectede.transform, playerDie.transform.position, clashKnockbackDistance, temp1));
                            yield return StartCoroutine(AdvanceTowards(selectedl.transform, selectede.transform.position, clashWinnerAdvanceDistance));
                        }
                        else if (temp2 > temp1)
                        {
                            if (clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().selected_card.data.dice[k].type == "blunt")
                            {
                                clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().blunt;
                            }
                            else if (clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().selected_card.data.dice[k].type == "pierce")
                            {
                                clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().pierce;
                            }
                            else if (clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().selected_card.data.dice[k].type == "slash")
                            {
                                clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().slash;
                            }
                            else if (clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().selected_card.data.dice[k].type == "guard")
                            {
                                clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().guard;
                            }

                            // If the winner's die is an evade, it deals no damage and the attacker shows "Missed"
                            if (enemyDie.selected_card != null && enemyDie.selected_card.data.dice[k].type == "evade")
                            {
                                selectedl.UpdateDI("Missed");
                            }
                            else if (k < playerDiceCount && playerDie.selected_card.data.dice[k].type == "block")
                            {
                                selectedl.health -= temp2 - temp1;
                                selectede.UpdateDI((temp2 - temp1).ToString());
                            }
                            else
                            {
                                selectedl.health -= temp2;
                                selectede.UpdateDI(temp2.ToString());
                            }

                            yield return StartCoroutine(Knockback(selectedl.transform, enemyDie.transform.position, clashKnockbackDistance, temp2));
                            yield return StartCoroutine(AdvanceTowards(selectede.transform, selectedl.transform.position, clashWinnerAdvanceDistance));
                        }
                        //wait and then reset indicators
                        yield return new WaitForSeconds(1);
                        selectedl.UpdateDI("");
                        selectede.UpdateDI("");
                        clashers[i].GetComponent<SpeedDie>().GetComponentInParent<Librarian>().spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().librarian.move;
                        clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().move;
                        
                    }
                    clashers[i].GetComponent<SpeedDie>().clashed = true;
                    clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().clashed = true;
                    clashers[i].GetComponent<SpeedDie>().GetComponentInParent<Librarian>().spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().librarian.normal;
                    clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().normal;
                    

                }
                //clash enemies
                else if (clashers[i].GetComponentInParent<Enemy>() != null)
                {
                    // per clasher

                    //per card played
                    selectede = clashers[i].GetComponentInParent<Enemy>();
                    selectedl = clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponentInParent<Librarian>();
                    EnemySpeedDie enemyDie = clashers[i].GetComponent<EnemySpeedDie>();
                    SpeedDie playerDie = enemyDie.clash_target;
                    int enemyDiceCount = enemyDie.selected_card.data.dice.Length;
                    int playerDiceCount = 0;
                    if (playerDie.selected_card == null)
                    {
                        playerDiceCount = 0;
                    }
                    else
                    {
                        playerDiceCount = playerDie.selected_card.data.dice.Length;
                    }
                    int maxDice = Mathf.Max(enemyDiceCount, playerDiceCount);
                    for (int k = 0; k < maxDice; k++)
                    {
                        //per die on card
                        int temp1 = 0;
                        if (k < enemyDiceCount)
                        {
                            temp1 = UnityEngine.Random.Range(enemyDie.selected_card.data.dice[k].min, enemyDie.selected_card.data.dice[k].max);
                        }
                        int temp2 = 0;
                        if (k < playerDiceCount && playerDie.clash_target == enemyDie)
                        {
                            temp2 = UnityEngine.Random.Range(playerDie.selected_card.data.dice[k].min, playerDie.selected_card.data.dice[k].max);
                        }

                        selectede.UpdateDI(temp1.ToString());
                        selectedl.UpdateDI(temp2.ToString());
                        yield return new WaitForSeconds(1);
                        selectede.UpdateDI("");
                        selectedl.UpdateDI("");

                        if (temp1 > temp2)
                        {
                            if (clashers[i].GetComponent<EnemySpeedDie>().selected_card.data.dice[k].type == "blunt")
                            {
                                clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().blunt;
                            }
                            else if (clashers[i].GetComponent<EnemySpeedDie>().selected_card.data.dice[k].type == "pierce")
                            {
                                clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().pierce;
                            }
                            else if (clashers[i].GetComponent<EnemySpeedDie>().selected_card.data.dice[k].type == "slash")
                            {
                                clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().slash;
                            }
                            else if (clashers[i].GetComponent<EnemySpeedDie>().selected_card.data.dice[k].type == "guard")
                            {
                                clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().guard;
                            }

                        if (temp1 > temp2)
                        {
                            // If winner (enemy) used evade, it deals no damage and attacker shows "Missed"
                            if (enemyDie.selected_card != null && enemyDie.selected_card.data.dice[k].type == "evade")
                            {
                                selectede.UpdateDI("Missed");
                            }
                            else if (k < playerDiceCount && playerDie.selected_card.data.dice[k].type == "block")
                            {
                                selectedl.health -= temp1 - temp2;
                                selectede.UpdateDI((temp1 - temp2).ToString());
                            }
                            else
                            {
                                selectedl.health -= temp1;
                                selectede.UpdateDI(temp1.ToString());
                            }

                            yield return StartCoroutine(Knockback(selectedl.transform, enemyDie.transform.position, clashKnockbackDistance));
                            yield return StartCoroutine(AdvanceTowards(selectede.transform, selectedl.transform.position, clashWinnerAdvanceDistance));
                        }
                        else if (temp2 > temp1)
                        {
                            if (enemyDie.clash_target.GetComponent<SpeedDie>().selected_card.data.dice[k].type == "blunt")
                            {
                                clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.blunt;
                            }
                            else if (enemyDie.clash_target.GetComponent<SpeedDie>().selected_card.data.dice[k].type == "pierce")
                            {
                                clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.pierce;
                            }
                            else if (enemyDie.clash_target.GetComponent<SpeedDie>().selected_card.data.dice[k].type == "slash")
                            {
                                clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.slash;
                            }
                            else if (enemyDie.clash_target.GetComponent<SpeedDie>().selected_card.data.dice[k].type == "guard")
                            {
                                clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.guard;
                            }
                            
                            // If winner (player) used evade, attacker shows "Missed" and no damage
                            if (playerDie.selected_card != null && playerDie.selected_card.data.dice[k].type == "evade")
                            {
                                selectede.UpdateDI("Missed");
                            }
                            else if (k < enemyDiceCount && enemyDie.selected_card.data.dice[k].type == "block")
                            {
                                selectede.health -= temp2 - temp1;
                                selectedl.UpdateDI((temp2 - temp1).ToString());
                            }
                            else
                            {
                                selectede.health -= temp2;
                                selectedl.UpdateDI(temp2.ToString());
                            }

                            yield return StartCoroutine(Knockback(selectede.transform, playerDie.transform.position, clashKnockbackDistance));
                            yield return StartCoroutine(AdvanceTowards(selectedl.transform, selectede.transform.position, clashWinnerAdvanceDistance));
                        }
                        //wait and then reset indicators
                        yield return new WaitForSeconds(1);
                        selectede.UpdateDI("");
                        selectedl.UpdateDI("");
                        clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().move;
                        clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.move;
                    }
                    clashers[i].GetComponent<EnemySpeedDie>().clashed = true;
                    clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().clashed = true;
                    clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().GetComponentInParent<Enemy>().normal;
                    clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.spriterenderer.sprite = clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().librarian.normal;
                }
                    
                

            }
        }
        
    //remove selected cards
    for (int i = 0; i < clashers.Count; i++)
    {
        if (clashers[i].GetComponent<SpeedDie>() != null)
        {
            clashers[i].GetComponent<SpeedDie>().selected_card = null;
            clashers[i].GetComponent<SpeedDie>().clash_target = null;
        }
        else if (clashers[i].GetComponent<EnemySpeedDie>() != null)
        {
            clashers[i].GetComponent<EnemySpeedDie>().selected_card = null;
            clashers[i].GetComponent<EnemySpeedDie>().clash_target = null;
        }
    }
    
    //next turn
    card_database.Clear();
    turn += 1;
    startcombat();
    }
    



    
}
