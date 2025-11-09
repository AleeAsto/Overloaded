using UnityEngine;

public class WateringCanBehaviour : GrabbableBehaviour
{
    [Header("Interacción")]
    public KeyCode waterKey = KeyCode.E;

    [HideInInspector] public bool nearPlant = false;

    [Header("Punta de la regadera (para buscar plantas cerca)")]
    public Transform tip;                     
    public float detectRadius = 0.7f;         
    public LayerMask plantMask;               

    [Header("Riego")]
    public float waterPerSecond = 1.5f;       
    public bool requireMovementToTilt = false;

    [Header("Tilt (inclinación visual)")]
    public float tiltAngle = -25f;            
    public float tiltSpeed = 10f;             

    [Header("FX (opcionales)")]
    public ParticleSystem waterFx;
    public AudioSource waterSfx;

    private Transform self;
    private float currentTilt = 0f;
    private bool isWatering = false;

    private bool nearInteractable = false;
    public override bool WantsToBlockDrop() => nearInteractable;
    public override string GetDropBlockHint() => nearInteractable ? "No puedes soltar aquí." : null;

    void Awake()
    {
        self = transform;
    }

    public override void OnPickedUp(PlayerGrab player)
    {
        isWatering = false;
        currentTilt = 0f;
        self.localRotation = Quaternion.identity;
        StopFx();
    }

    public override void OnDropped(PlayerGrab player)
    {
        isWatering = false;
        currentTilt = 0f;
        self.localRotation = Quaternion.identity;
        StopFx();
    }

    public override void OnCarriedUpdate(PlayerGrab player, Vector2 moveInput)
    {
        bool keyHeld = Input.GetKey(waterKey);

        // ===========================
        //  DETECCIÓN DE PLANTAS
        // ===========================
        Plant target = null;
        nearPlant = false;

        if (tip != null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(tip.position, detectRadius, plantMask);
            float best = float.MaxValue;

            foreach (var h in hits)
            {
                var p = h.GetComponent<Plant>() ?? h.GetComponentInParent<Plant>();
                if (p == null || p.IsCompleted) continue;

                float d = (tip.position - h.bounds.ClosestPoint(tip.position)).sqrMagnitude;
                if (d < best)
                {
                    best = d;
                    target = p;
                    nearPlant = true;
                }
            }
        }

        nearInteractable = (target != null);
        // PlayerGrab usará WantsToBlockDrop() y devolverá true si nearInteractable es true

        // ===========================
        //  LÓGICA DE RIEGO
        // ===========================
        bool moving = Mathf.Abs(moveInput.x) > 0.01f || Mathf.Abs(moveInput.y) > 0.01f;
        bool canTilt = !requireMovementToTilt || moving;

        isWatering = keyHeld && target != null;

        if (isWatering)
        {
            target.ApplyWater(waterPerSecond * Time.deltaTime);

            if (canTilt)
            {
                currentTilt = Mathf.Lerp(currentTilt, tiltAngle, Time.deltaTime * tiltSpeed);
                self.localRotation = Quaternion.Euler(0, 0, currentTilt);
            }

            PlayFx();
        }
        else
        {
            currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * tiltSpeed);
            self.localRotation = Quaternion.Euler(0, 0, currentTilt);

            if (!keyHeld)
                StopFx();
        }
    }

    void PlayFx()
    {
        if (waterFx != null && !waterFx.isPlaying) waterFx.Play();
        if (waterSfx != null && !waterSfx.isPlaying) waterSfx.Play();
    }

    void StopFx()
    {
        if (waterFx != null && waterFx.isPlaying) waterFx.Stop();
        if (waterSfx != null && waterSfx.isPlaying) waterSfx.Stop();
    }

    void OnDrawGizmosSelected()
    {
        if (tip == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(tip.position, detectRadius);
    }
}
