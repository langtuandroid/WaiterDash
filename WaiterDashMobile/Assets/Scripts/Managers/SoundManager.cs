using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CustomerWaitingQueue.Instance.OnCustomerArrivedAtFrontOfQueue += Instance_OnCustomerArrivedAtFrontOfQueue;
        TrashCounter.OnTrashThrown += OnTrashThrown;
        Player.Instance.OnDroppedSomething += Player_OnDroppedSomething;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseDiningTable.OnCustomerPayment += OnCustomerPayment;
        KitchenFoodCounter.Instance.OnFoodReady += KitchenFoodCounter_OnFoodReady;
    }

    private void KitchenFoodCounter_OnFoodReady(object sender, System.EventArgs e)
    {
        KitchenFoodCounter kitchenFoodCounter = KitchenFoodCounter.Instance;
        PlaySound(audioClipRefsSO.KitchenFoodReady, kitchenFoodCounter.transform.position);
    }

    private void OnCustomerPayment(object sender, System.EventArgs e)
    {
        BaseDiningTable diningTable = sender as BaseDiningTable;
        PlaySound(audioClipRefsSO.CustomerPayment, diningTable.transform.position);

    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        Player player = Player.Instance;
        PlaySound(audioClipRefsSO.PlayerPicked, player.transform.position);
    }

    private void Player_OnDroppedSomething(object sender, System.EventArgs e)
    {
        Player player = Player.Instance;
        PlaySound(audioClipRefsSO.PlayerThrown, player.transform.position);
    }

    private void OnTrashThrown(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.TrashThrown, trashCounter.transform.position);
    }

    private void Instance_OnCustomerArrivedAtFrontOfQueue(object sender, System.EventArgs e)
    {
        // CasaCounter casaCounter = CasaCounter.Instance;
        // PlaySound(audioClipRefsSO.CustomerArrived, casaCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayerFootstepsSound(Vector3 position, float volume) {
        PlaySound(audioClipRefsSO.PlayerWalk, position, volume);
    }
    
    public void TaskUnsuccessFull(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.PlayerTaskUnsuccessFull, position, volume);
    }

    public void TaskSuccessFull(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.PlayerTasksuccessFull, position, volume);
    }
}
