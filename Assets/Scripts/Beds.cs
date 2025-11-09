using UnityEngine;

public class Beds : MonoBehaviour
{
    //Tiempo de cambio a la siguiente animacion
    [SerializeField]float _timeChangeAnimatino = 0.01f;

    bool _isMade = false;
    [SerializeField] float _madeDuration;
        

    [SerializeField] KeyCode _key = KeyCode.E;



    [SerializeField] Collider2D col;
    Animator anim;

    [Header (" Manager UI")]
    [SerializeField]BedsManager manager;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (anim == null) anim = GetComponent<Animator>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (manager == null) manager = FindFirstObjectByType<BedsManager>();
        manager?.RegisterBed(this);
    }

    void MadeBed()
    {
        anim.cros
    }
}
