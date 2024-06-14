using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class UnityCloudSave : MonoBehaviour
{
    public static UnityCloudSave Instance;
    public static EventHandler onSavedCloudFileLoaded;
    private void Awake()
    {
        Instance = this;
        loadCloudSavedFile();
    }
    void Start()
    {
    }

    private async void loadCloudSavedFile()
    {
        try
        {
            if (!ES3.FileExists("GameSaveData.es3"))
            {
                try
                {
                    ES3.Save("PlayerId", AuthenticationService.Instance.PlayerId);
                }
                catch (Exception saveException)
                {
                    Debug.LogError($"Failed to save PlayerId: {saveException.Message}");
                    // Optionally, you can handle this error further or return/exit the method
                    return;
                }
            }

            byte[] data = null;
            try
            {
                data = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("GameSaveData");
            }
            catch (Exception loadException)
            {
                Debug.LogError($"Failed to load cloud save data: {loadException.Message}");

                // Invoke to load from local file.
                onSavedCloudFileLoaded?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (data != null)
            {
                try
                {
                    File.WriteAllBytes(Application.persistentDataPath + "/GameSaveData.es3", data);
                    onSavedCloudFileLoaded?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception writeException)
                {
                    Debug.LogError($"Failed to write save data to file: {writeException.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An unexpected error occurred: {ex.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        saveCloudSavedFile();
    }

    public void saveCloudSavedFile()
    {
        CloudSaveService.Instance.Files.Player.SaveAsync("GameSaveData", ES3.LoadRawBytes("GameSaveData.es3"));
    }
}
