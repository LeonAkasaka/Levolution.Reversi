using UnityEngine;

namespace Levolution.Reversi.Components
{
    public class TableCommandBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TableView _table = null;

        public TableCommands TableCommands { get; private set; }

        private void Start()
        {
            TableCommands = new TableCommands(_table.Table, _table.FirstPlayer);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { TableCommands.MoveLeft(); }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { TableCommands.MoveRight(); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { TableCommands.MoveUp(); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { TableCommands.MoveDown(); }
            if (Input.GetKeyDown(KeyCode.Return)) { TableCommands.Place(); }
        }
    }
}
