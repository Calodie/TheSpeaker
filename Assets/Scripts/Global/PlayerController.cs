using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class PlayerController
    {
        private UnitMovable player;

        public void Update(UnitMovable player)
        {
            this.player = player;
            Interact();
            Move();
            Shoot();
            Test_Immortal();
        }

        /// <summary>
        ///  交互检查
        /// </summary>
        private void Interact()
        {
            if (!player)
            {
                return;
            }

            Camera camera = CameraController.instance._camera;

            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

            Unit pointingUnit = null;
            Interactable interactable = null;
            float closestDistance = float.MaxValue;
            foreach (Unit unit in Unit.units)
            {
                float mouseDistance = Vector2.Distance(unit.transform.position, mousePos);
                // 寻找与光标最近的unit
                if (mouseDistance < unit.maxCursorDistance && mouseDistance < closestDistance)
                {
                    pointingUnit = unit;
                    closestDistance = mouseDistance;

                    // 检查是否可交互
                    interactable = null;
                    Interactable thisInteractable = unit.GetComponent<Interactable>();
                    if (thisInteractable)
                    {
                        float distance = Vector2.Distance(unit.transform.position, player.transform.position);
                        if (distance < thisInteractable.maxDistance)
                        {
                            interactable = thisInteractable;
                        }
                    }
                }
            }

            UIIngame.instance.cursor.SetPointingUnit(pointingUnit, interactable);

            // 交互
            if (interactable && Input.GetKeyDown(KeyCode.E) && !GameManager.instance.Paused && !UIDialog.instance.Speaking)
            {
                interactable.Interact();
            }
        }

        private void Move()
        {
            if (!player)
            {
                return;
            }

            Vector3 direction = Vector3.zero;

            if (!GameManager.instance.Paused && !UIDialog.instance.Speaking)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    direction += Vector3.up;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    direction += Vector3.down;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    direction += Vector3.left;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    direction += Vector3.right;
                }

                if (direction.magnitude > 0.1f)
                {
                    direction = direction.normalized;
                }
                else
                {
                    direction = Vector3.zero;
                }
            }

            player.MoveForce(direction);
        }

        private void Shoot()
        {
            if (player && Input.GetKey(KeyCode.Mouse0) && !GameManager.instance.Paused && !UIDialog.instance.Speaking)
            {
                Vector3 mousePos = CameraController.instance._camera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = ((Vector2)mousePos - (Vector2)player.transform.position).normalized;
                player._attacker.StartAttack(direction);
            }
        }

        private void Test_Immortal()
        {
            if (player && Input.GetKeyDown(KeyCode.Q))
            {
                player._unitStatsManager.immortal = !player._unitStatsManager.immortal;
            }
        }
    }
}
