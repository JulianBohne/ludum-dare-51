using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class StormScript : MonoBehaviour
{
    public static int timesLoaded = 0;

    private ParticleSystem storm;
    private float remainingTime;

    public TextMeshProUGUI counter;

    private PostProcessVolume ppv;

    private Bloom bloom;

    private CameraShake shake;

    private AudioManager audioManager;

    public TextMeshProUGUI dayCounter;

    public TextMeshProUGUI timerGUI;
    private float timer;

    private Healthbar health;
    private PlayerController player;

    bool dayEnded;

    private void Awake()
    {
        timesLoaded++;
    }

    void Start()
    {
        storm = GetComponent<ParticleSystem>();
        remainingTime = 10f;
        shake = FindObjectOfType<CameraShake>();

        bloom = ScriptableObject.CreateInstance<Bloom>();
        bloom.enabled.Override(true);
        bloom.intensity.Override(0f);
        ppv = PostProcessManager.instance.QuickVolume(LayerMask.NameToLayer("Post Processing"), 100f, bloom);

        audioManager = FindObjectOfType<AudioManager>();
        dayCounter.SetText("Day " + timesLoaded);

        timer = 60;
        health = FindObjectOfType<Healthbar>();
        player = FindObjectOfType<PlayerController>();
        dayEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        remainingTime -= Time.deltaTime;
        timer -= Time.deltaTime;

        counter.SetText(Mathf.CeilToInt(remainingTime).ToString());
        if(remainingTime < 0 && !storm.isPlaying)
        {
            storm.Play();
            audioManager.play("LowRumble");
            StartCoroutine(rampBloomTo(25f, 0.1f));
            StartCoroutine(shake.Shake(10, 0.1f));
        }

        if (storm.isPlaying)
        {
            if(player.currentState != PlayerController.State.SHELTER)
            {
                health.Health -= 10f*Time.deltaTime;
            }
        }

        if(timer < 0 && !dayEnded)
        {
            dayEnded = true;
            health.Health -= 5f * Time.deltaTime;
            storm.Play();
            audioManager.play("LowRumble");
            StartCoroutine(rampBloomTo(25f, 0.1f));
            StartCoroutine(shake.Shake(10, 0.1f));
        }
        if(timer < 0) health.Health -= 50f * Time.deltaTime;

        timerGUI.SetText(Mathf.CeilToInt(timer).ToString());
    }

    private IEnumerator rampBloomTo(float value, float duration)
    {
        float elapsedTime = 0f;
        float startBloom = bloom.intensity.value;

        while (elapsedTime < duration)
        {

            bloom.intensity.Override(Mathf.Lerp(startBloom, value, elapsedTime / duration));

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    public void OnParticleSystemStopped()
    {
        StartCoroutine(rampBloomTo(0f, 1f));
        remainingTime = 10f;
        Debug.Log("Stop");
    }

    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(ppv, true, true);
    }

}
