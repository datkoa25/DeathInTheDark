using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{
    public int VidaMax;
    public float VidaActual;
    public Image BarraDeVida;
    private Animator anim;
    
    void Start()
    {
        VidaActual = VidaMax;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        RevisarVida();
        
        if (VidaActual <= 0)
        {
            anim.SetTrigger("Death");
        }    
    }

    public void RevisarVida()
    {
        BarraDeVida.fillAmount = VidaActual / VidaMax;
    }
}