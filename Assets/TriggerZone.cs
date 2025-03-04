using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private TriggerDialog _dialog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "Player")
        //{
            Dialogues.System.DialogSystem.Instance.StartDialog(_dialog.DialogKeyEnter, _dialog.DialogPathEnter);
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.tag == "Player")
        //{
            Dialogues.System.DialogSystem.Instance.StartDialog(_dialog.DialogKeyExit, _dialog.DialogPathExit);
        //}
    }
}