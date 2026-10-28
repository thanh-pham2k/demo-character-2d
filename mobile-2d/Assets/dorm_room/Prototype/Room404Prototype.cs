using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

namespace DormRoom.Prototype
{
    // Temporary in-memory inspection flow. No persistence or connection to game saves.
    public sealed class Room404Prototype : MonoBehaviour
    {
        public CharacterController player;
        public Transform leftChair, rightChair, leftStudy, rightStudy, leftClimb, rightClimb, leftSleep, rightSleep, leftExit, rightExit, doorPoint, doorPanel;
        const float HalfHeight = .85f;
        Vector3 initialPosition, panelPosition;
        Quaternion initialRotation, panelRotation;
        Vector3 initialScale;
        Canvas canvas;
        Text label;
        bool menuOpen, doorOpen, seated, studying, sleeping, transitioning;
        Transform activeBed, activeExit, activeStudy;

        void Awake()
        {
            initialPosition = player.transform.position;
            initialRotation = player.transform.rotation;
            initialScale = player.transform.localScale;
            panelPosition = doorPanel.localPosition;
            panelRotation = doorPanel.localRotation;
            CreateOverlay();
        }

        void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;
            if (keyboard.f8Key.wasPressedThisFrame) { menuOpen = !menuOpen; RefreshOverlay(); }
            if (menuOpen)
            {
                if (keyboard.digit1Key.wasPressedThisFrame) Checkpoint(0);
                else if (keyboard.digit2Key.wasPressedThisFrame) Checkpoint(1);
                else if (keyboard.digit3Key.wasPressedThisFrame) Checkpoint(2);
                else if (keyboard.digit4Key.wasPressedThisFrame) Checkpoint(3);
                else if (keyboard.digit5Key.wasPressedThisFrame) Checkpoint(4);
                else if (keyboard.digit6Key.wasPressedThisFrame) Checkpoint(5);
                return;
            }
            if (keyboard.eKey.wasPressedThisFrame && !transitioning) Interact();
            if (seated || sleeping || transitioning) return;
            var move = new Vector3((keyboard.dKey.isPressed ? 1 : 0) - (keyboard.aKey.isPressed ? 1 : 0), 0,
                                   (keyboard.wKey.isPressed ? 1 : 0) - (keyboard.sKey.isPressed ? 1 : 0));
            player.Move((move.normalized * 2f + Vector3.down * 2f) * Time.deltaTime);
        }

        void Checkpoint(int index)
        {
            StopAllCoroutines(); seated = false; studying = false; sleeping = false; transitioning = false; activeBed = null; activeExit = null; activeStudy = null;
            player.enabled = false;
            player.transform.SetPositionAndRotation(initialPosition, initialRotation);
            player.transform.localScale = initialScale;
            doorPanel.SetLocalPositionAndRotation(panelPosition, panelRotation); doorOpen = false;
            Transform anchor = null;
            switch (index) { case 1: anchor = leftChair; break; case 2: anchor = rightChair; break; case 3: anchor = leftClimb; break; case 4: anchor = rightClimb; break; case 5: anchor = doorPoint; break; }
            if (anchor != null) player.transform.position = new Vector3(anchor.position.x, HalfHeight, anchor.position.z - .75f);
            player.enabled = true; menuOpen = false; RefreshOverlay();
        }

        void Interact()
        {
            if (sleeping)
            {
                StartCoroutine(LeaveBed()); return;
            }
            if (seated && !studying)
            {
                if (activeStudy == null) return;
                Vector3 direction = Vector3.ProjectOnPlane(activeStudy.position - player.transform.position, Vector3.up);
                if (direction.sqrMagnitude > .0001f) player.transform.rotation = Quaternion.LookRotation(direction);
                studying = true; RefreshOverlay(); return;
            }
            if (seated)
            {
                Vector3 exit = activeExit != null ? activeExit.position : player.transform.position;
                seated = false; studying = false; sleeping = false; activeBed = null; activeExit = null; activeStudy = null;
                player.transform.localScale = initialScale; player.transform.rotation = initialRotation;
                Teleport(new Vector3(exit.x, HalfHeight, exit.z - .65f)); RefreshOverlay(); return;
            }
            Transform chosen = null; float distance = 1.35f;
            foreach (var point in new[] { leftChair, rightChair, leftClimb, rightClimb, doorPoint })
            {
                float d = Vector2.Distance(new Vector2(point.position.x, point.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
                if (d < distance) { distance = d; chosen = point; }
            }
            if (chosen == null) return;
            if (chosen == doorPoint)
            {
                doorOpen = !doorOpen;
                var angle = doorOpen ? 90f : 0f;
                var hinge = panelPosition + Vector3.left * .75f;
                var turn = Quaternion.Euler(0, angle, 0);
                doorPanel.SetLocalPositionAndRotation(hinge + turn * (panelPosition - hinge), turn * panelRotation);
                return;
            }
            if (chosen == leftChair || chosen == rightChair)
            {
                seated = true; player.transform.localScale = new Vector3(initialScale.x, initialScale.y * .58f, initialScale.z);
                activeStudy = chosen == leftChair ? leftStudy : rightStudy;
                Teleport(new Vector3(chosen.position.x, chosen.position.y + HalfHeight * .58f, chosen.position.z - .35f)); RefreshOverlay(); return;
            }
            activeBed = chosen == leftClimb ? leftSleep : rightSleep;
            activeExit = chosen == leftClimb ? leftExit : rightExit;
            StartCoroutine(ClimbToBed(chosen));
        }
        IEnumerator MoveTo(Vector3 target, float duration)
        {
            Vector3 start = player.transform.position;
            for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
            {
                player.transform.position = Vector3.Lerp(start, target, elapsed / duration);
                yield return null;
            }
            player.transform.position = target;
        }
        IEnumerator ClimbToBed(Transform climb)
        {
            transitioning = true; player.enabled = false;
            Vector3 foot = new Vector3(climb.position.x, HalfHeight, climb.position.z);
            yield return MoveTo(foot, .4f);
            yield return MoveTo(new Vector3(foot.x, activeBed.position.y + .23f, foot.z), .8f);
            player.transform.rotation = Quaternion.Euler(0, 0, 90);
            yield return MoveTo(activeBed.position + Vector3.up * .23f, .4f);
            sleeping = true; transitioning = false;
        }
        IEnumerator LeaveBed()
        {
            transitioning = true; sleeping = false;
            Vector3 exit = activeExit.position;
            yield return MoveTo(new Vector3(exit.x, player.transform.position.y, exit.z), .4f);
            player.transform.rotation = initialRotation;
            yield return MoveTo(new Vector3(exit.x, HalfHeight, exit.z), .8f);
            yield return MoveTo(new Vector3(exit.x, HalfHeight, exit.z - .65f), .4f);
            player.enabled = true; activeBed = null; activeExit = null; transitioning = false;
        }
        void Teleport(Vector3 position) { player.enabled = false; player.transform.position = position; player.enabled = true; }

        void CreateOverlay()
        {
            var host = new GameObject("PrototypeCheckpointOverlay", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            host.transform.SetParent(transform, false); canvas = host.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var textObject = new GameObject("Instructions", typeof(RectTransform), typeof(Text)); textObject.transform.SetParent(host.transform, false);
            label = textObject.GetComponent<Text>(); label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); label.fontSize = 18; label.color = Color.white;
            label.alignment = TextAnchor.UpperLeft; label.horizontalOverflow = HorizontalWrapMode.Wrap; label.verticalOverflow = VerticalWrapMode.Overflow;
            var rect = label.rectTransform; rect.anchorMin = new Vector2(0,1); rect.anchorMax = new Vector2(0,1); rect.pivot = new Vector2(0,1); rect.anchoredPosition = new Vector2(12,-12); rect.sizeDelta = new Vector2(600,260);
            RefreshOverlay();
        }
        void RefreshOverlay()
        {
            label.text = menuOpen ? "F8 CHECKPOINT — bam so\n1: Giua phong\n2: Ban trai\n3: Ban phai\n4: Thang trai\n5: Thang phai\n6: Cua" : seated ? (studying ? "Dang hoc (thu nghiem) | E: dung day | F8: checkpoint" : "Dang ngoi | E: hoc | F8: checkpoint") : "WASD: di chuyen | E: tuong tac | F8: checkpoint";
        }
    }
}
