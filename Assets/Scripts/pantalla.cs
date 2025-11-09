using UnityEngine;
using UnityEngine.SceneManagement;
public class pantalla : MonoBehaviour
{
    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void win() => ani.CrossFade("Win", 0.01f);
    public void lose() => ani.CrossFade("lose", 0.01f);

    public void restart() => SceneManager.LoadScene("escena");
}
