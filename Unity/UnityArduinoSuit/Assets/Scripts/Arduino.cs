using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class Arduino : MonoBehaviour
{
    public string PortName = "COM1";
    public int BaundRate = 9600;

    /// Чтение данных из ардуино в unity
    public static SerialPort arduinoPort;

    public static void SentToArduino(string message)
    {
        if (!arduinoPort.IsOpen) return;

        arduinoPort.Write(message);
    }

    private void Start()
    {
        arduinoPort = new SerialPort(PortName, BaundRate);
        arduinoPort.Open();
    }

    /* Чтение данных с Arduino (Пробная версия)
    private void Start()
    {
        arduinoPort.Open();
        PortWriteThread = new Thread(ReadArduino);
        PortWriteThread.Start();
        loop = true;
    }

    /// <summary>
    /// Поток для считывания без зависаний unity
    /// </summary>
    private Thread PortWriteThread;
    /// <summary>
    /// Для завершения цикла в потоке.
    /// Не удалять! Это сделанно, чтобы небыло зависаний при выходе из игры
    /// </summary>
    private bool loop;

    private void OnDestroy()
    {
        loop = false;
        PortWriteThread.Join();
        PortWriteThread.Abort();
        arduinoPort.Close();
    }

    void ReadArduino()
    {
        if (arduinoPort == null || !arduinoPort.IsOpen) Debug.Log("Проблема с подключение к порту");

        while (loop)
        {
            var strData = arduinoPort.ReadLine();
            Debug.Log(strData);
            Thread.Sleep(0);
        }
    }
    */
}