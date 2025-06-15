using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [SerializeField] private GameObject LEnemy;
    [SerializeField] private GameObject FEnemy;
    [SerializeField] private GameObject REnemy;
    [SerializeField] private TextMeshProUGUI deathsText;
    Dictionary<int, GameObject> prefabs;

    private bool IsGameStarted = false;
    private bool[] enemysOchered = {false, false, false};
    public int deaths = 0;
    // Start is called before the first frame update
    void Start()
    {
        prefabs = new Dictionary<int, GameObject>()
        {
            { 0, LEnemy},
            { 1, FEnemy},
            { 2, REnemy}
        };

        enemysOchered[1] = true;
        GameObject first =  Instantiate(FEnemy);
        Transform firstCanvas = first.transform.Find("Canvas");
        firstCanvas.gameObject.SetActive(false);

        StartCoroutine(StartGame(firstCanvas));
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameStarted)
        {
            if (enemysOchered.Select(b => b ? 1 : 0).Sum() >= 1)
            {
                int i = Random.Range(0, 3);
                if (enemysOchered[i] == false)
                {
                    enemysOchered[i] = true;
                    Instantiate(prefabs[i]);
                }
            }
        } 
    }

    public void DelEnemy(int index)
    {
        enemysOchered[index] = false;
        deaths++;
        deathsText.text = deaths.ToString();
    }

    private IEnumerator StartGame(Transform firstCanvas)
    {
        yield return new WaitForSeconds(7.1f);
        IsGameStarted = true;
        firstCanvas.gameObject.SetActive(true);
    }
}
