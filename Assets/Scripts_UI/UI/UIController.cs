using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        if (instance == null )
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [Header("UI GameObjects")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameoverScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] public GameObject finalWin;
    [SerializeField] public GameObject player;
    [Header("UI healthSlider")]
    [SerializeField] private Slider healthSlider;
    
    [Header("UI LanternSlider")]
    [SerializeField] private Slider lanternSlider;
    
    [Header("UI Heart Piece")]
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currentHealthBar;

    [Header("UI KeyHolder")]
    [SerializeField] private Image totalCollectibleBar;
    [SerializeField] private Image currentCollectibleBar;
    
    [Header("UI Return MainMenu")]
    [SerializeField] private GameObject[] DestroyOnLoadObjects;


    void Start()
    {        
        pauseScreen.SetActive(false);
        gameoverScreen.SetActive(false);
        winScreen.SetActive(false);
        finalWin=GameObject.FindWithTag("Final");
        finalWin=GameObject.FindWithTag("Player");
    }

    void Update()
    {

        // Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseScreen.activeInHierarchy)
                PauseMenu(true);
            else
                PauseMenu(false);
        }
        if(Vector3.Distance(player.transform.position,finalWin.transform.position)<=10)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        totalhealthBar.fillAmount = maxHealth / 10;
        currentHealthBar.fillAmount = currentHealth / 10;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    public void UpdateLantern(float currentLight, float maxLight)
    {
        lanternSlider.maxValue = maxLight;
        lanternSlider.value = currentLight;
    }

    public void UpdateCollectibleHolder(float currentAmountCollect, float maxAmountCollect)
    {
        totalCollectibleBar.fillAmount = maxAmountCollect;
        currentCollectibleBar.fillAmount = currentAmountCollect;
    }

    // UI Pause Menu

    public void PauseMenu(bool statut)
    {
        pauseScreen.SetActive(statut);
        Time.timeScale = System.Convert.ToInt32(!statut);
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        // Gets a new DontDestroyOnLoad
        instance = null;
        Destroy(gameObject);

        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }


    public void ReturnToMenu()
    {
        // Set instance to null to release the reference
        instance = null;

        // Destroy All Objects in DontDestroyOnLoad
        foreach (GameObject obj in DestroyOnLoadObjects)
        {
            Destroy(obj);
        }

        SceneManager.LoadScene(0);
    }

    //IEnumerator LoadMenuAsync()
    //{
    //    // Asynchronously load the main menu scene 
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

    //    // Wait until the asynchronous operation is complete
    //    while (!asyncLoad.isDone)
    //    {
    //        yield return null;
    //    }
    //}


    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exit option for editor 
#endif
    }


}
