using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class HahaCakesGoBrrr : MonoBehaviour
{
    [SerializeField, Tooltip("the rectangle in which the cakes will go brrr")]
    private Rect m_rect;

    [SerializeField, Tooltip("the gameOver tmp element")]
    private TextMeshProUGUI m_text;
    
    [SerializeField, Tooltip("the cake prefab")]
    private GameObject m_cakePrefab;
    
    [SerializeField, Tooltip("the time between cakes")]
    private float m_timeBetweenCakes = .1f;


    [SerializeField, Tooltip("the high score audio source")]
    private AudioSource m_highScoreAudio;
    
    [SerializeField, Tooltip("the high score audio source")]
    private AudioSource m_ClappingAudio;
    
    [SerializeField, Tooltip("the cake adding sound audio source")]
    private AudioSource m_cakeAudio;
    
    
    
    private void Start()
    {
        FunctionToMakeCakesGoBrr(PlayerPrefs.GetInt("LastScore"));
    }
    // Update is called once per frame
    public void FunctionToMakeCakesGoBrr(int p_score)
    {
        m_text.text = $"Game Over\nScore {0}\nHigh Score : {PlayerPrefs.GetInt("HighScore")}";
        StartCoroutine(MakeCakesGoBr(p_score));
    }

    private bool m_canRestart = false;
    
    IEnumerator MakeCakesGoBr(int pScore)
    {
        yield return new WaitForSeconds(2f);
        int cakesBrrrd = 0;
        while (cakesBrrrd < pScore)
        {
            yield return new WaitForSeconds(m_timeBetweenCakes);
            Vector3 position = new Vector3(m_rect.x + Random.Range(m_rect.xMin, m_rect.xMax), 0, m_rect.y + Random.Range(m_rect.yMin, m_rect.yMax));

            if (Physics.SphereCast(position + Vector3.up * 100f,0.62f, Vector3.down, out RaycastHit hit, 150f))
            {
                Instantiate(m_cakePrefab, hit.point + Vector3.up * 0.275f, m_cakePrefab.transform.rotation);
            }

            cakesBrrrd++;
            String highScoreText = cakesBrrrd < PlayerPrefs.GetInt("HighScore") ? $" High Score : {PlayerPrefs.GetInt("HighScore")}" : "New High Score !";
            
            if(PlayerPrefs.GetInt("HighScore") == cakesBrrrd)
            {
                m_highScoreAudio.Play();
                m_ClappingAudio.Play();
            }
            else m_cakeAudio.Play();
            
            m_text.text = $"Game Over\nScore {cakesBrrrd}\n{highScoreText}";
        }

        m_canRestart = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) SceneManager.LoadScene(1);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = new Vector3(m_rect.x, 1, m_rect.y);
        Debug.DrawLine(pos + new Vector3(m_rect.xMin,0,m_rect.yMin),pos + new Vector3(m_rect.xMin,0,m_rect.yMax));
        Debug.DrawLine(pos + new Vector3(m_rect.xMax,0,m_rect.yMin),pos + new Vector3(m_rect.xMax,0,m_rect.yMax));
        
        Debug.DrawLine(pos + new Vector3(m_rect.xMin,0,m_rect.yMin),pos + new Vector3(m_rect.xMax,0,m_rect.yMin));
        Debug.DrawLine(pos + new Vector3(m_rect.xMin,0,m_rect.yMax),pos + new Vector3(m_rect.xMax,0,m_rect.yMax));
    }
}
