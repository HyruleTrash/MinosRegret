using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    public Text PlayerTextPlane;

    public string[] TextProgression;
    public int Count = 0;
    public string Name;
    public int TimeMax = 2;
    public Color NpcColor;

    int currentchar = 0;
    bool playing = false;
    bool TimerPlaying = false;
    float TimeLeft = 0;
    float OriginalAlpha;

    private void Start()
    {
        OriginalAlpha = PlayerTextPlane.color.a;
    }

    public void Interact()
    {
        if (!playing)
        {
            if (Count < TextProgression.Length)
            {
                PlayerTextPlane.text = "";
                TimerPlaying = false;
                playing = true;
                currentchar = 0;
                switch (Name)
                {
                    case "Bull":
                        if (Count == 3)
                        {
                            Count++;
                        }
                        else
                        {
                            Count++;
                        }
                        break;
                    default:
                        Count++;
                        break;
                }
                PlayerTextPlane.color = NpcColor;
                PlayerTextPlane.color = new Color(NpcColor.r, NpcColor.g, NpcColor.b, OriginalAlpha);
            }
            else
            {
                PlayerTextPlane.text = "";
                TimerPlaying = false;
                playing = true;
                currentchar = 0;
                PlayerTextPlane.color = NpcColor;
                PlayerTextPlane.color = new Color(NpcColor.r, NpcColor.g, NpcColor.b, OriginalAlpha);
            }
        }
    }

    public void TimerDone()
    {
        switch (Name)
        {
            case "Bull":
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (TimerPlaying)
        {
            TimeLeft += Time.deltaTime;
            float Alpha = (1 - (((100 / TimeMax) * TimeLeft) / 100)) * OriginalAlpha;
            PlayerTextPlane.color = new Color(PlayerTextPlane.color.r, PlayerTextPlane.color.g, PlayerTextPlane.color.b, Alpha);
            if (TimeLeft > TimeMax)
            {
                TimerDone();
                TimerPlaying = false;
            }
        }

        if (playing)
        {
            PlayerTextPlane.text += TextProgression[Count - 1][currentchar];
            currentchar++;
            if (currentchar >= TextProgression[Count - 1].Length)
            {
                if (Count < TextProgression.Length)
                {
                    PlayerTextPlane.text += System.Environment.NewLine + "[Interact for more]";
                }
                playing = false;
                TimerPlaying = true;
                TimeLeft = 0;
            }
            PlayerTextPlane.text = PlayerTextPlane.text.Replace("@", System.Environment.NewLine);
        }
    }
}
