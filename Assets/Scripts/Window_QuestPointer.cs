using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_QuestPointer : MonoBehaviour 
{

    [SerializeField] private Camera uiCamera;
    [SerializeField] private Sprite arrowSprite;

    private List<QuestPointer> questPointerList;

    private void Awake() 
    {
        questPointerList = new List<QuestPointer>();
    }

    private void Update() 
    {
        foreach (QuestPointer questPointer in questPointerList) 
        {
            questPointer.Update();
        }
    }

    public QuestPointer CreatePointer(Vector3 targetPosition) 
    {
        GameObject pointerGameObject = Instantiate(transform.Find("pointerTemplate").gameObject);
        pointerGameObject.SetActive(true);
        pointerGameObject.transform.SetParent(transform, false);
        QuestPointer questPointer = new QuestPointer(targetPosition, pointerGameObject, uiCamera, arrowSprite);
        questPointerList.Add(questPointer);
        return questPointer;
    }

    public void DestroyPointer(QuestPointer questPointer) 
    {
        questPointerList.Remove(questPointer);
        questPointer.DestroySelf();
    }

    public class QuestPointer 
    {

        private Vector3 targetPosition;
        private GameObject pointerGameObject;
        private Sprite arrowSprite;
        private Camera uiCamera;
        private RectTransform pointerRectTransform;
        private Image pointerImage;


        public QuestPointer(Vector3 targetPosition, GameObject pointerGameObject, Camera uiCamera, Sprite arrowSprite) 
        {
            this.targetPosition = targetPosition;
            this.pointerGameObject = pointerGameObject;
            this.uiCamera = uiCamera;
            this.arrowSprite = arrowSprite;
            
            pointerRectTransform = pointerGameObject.GetComponent<RectTransform>();
            pointerImage = pointerGameObject.GetComponent<Image>();
        }

        public void Update() 
        {
            float borderSize = 100f;
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
            bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

            if (isOffScreen) 
            {
                RotatePointerTowardsTargetPosition();

                pointerGameObject.SetActive(true);
                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                //cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
                //cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);

                //Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                //pointerRectTransform.position = pointerWorldPosition;
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
            }
            else 
            {
                pointerGameObject.SetActive(false);
            }
        }

        private void RotatePointerTowardsTargetPosition() 
        {
            Vector3 toPosition = targetPosition;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.y = 0f;
            Vector3 dir = (toPosition - fromPosition).normalized;
            float angle = (Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg) % 360;
            pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle );
        }

        public void DestroySelf() 
        {
            Destroy(pointerGameObject);
        }

    }
}
