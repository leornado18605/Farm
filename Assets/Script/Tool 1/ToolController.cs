using UnityEngine;
using UnityEngine.EventSystems;

public class ToolController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private LayerMask soilLayer;

    private GameObject targetSoil;
    private ToolType currentTool = ToolType.Hoe;

    void Update()
    {
        if (IsPointerOverUI()) return;
        if (Input.GetMouseButtonDown(0))
            TryUseTool();
    }

    private bool IsPointerOverUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
        if (EventSystem.current == null) return false;
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            return EventSystem.current.IsPointerOverGameObject(t.fingerId);
        }
        return false;
#else
        return false;
#endif
    }

    private void TryUseTool()
    {
        if (currentTool != ToolType.Hoe) return;

        Vector2 clickPoint;
        if (!TryGetClickPoint(out clickPoint)) return;

        if (!TrySelectSoil(clickPoint)) return;

        FacePlayerToTarget(clickPoint);
        TryStartHoeing();
    }

    
    private bool TryGetClickPoint(out Vector2 clickPoint)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPoint = new Vector2(mousePos.x, mousePos.y);
        return true;
    }
    
    private bool TrySelectSoil(Vector2 clickPoint)
    {
        Collider2D hit = Physics2D.OverlapPoint(clickPoint, soilLayer);
        if (hit == null || !hit.CompareTag("Soil"))
            return false;

        targetSoil = hit.gameObject;
        return true;
    }
    
    private void FacePlayerToTarget(Vector2 clickPoint)
    {
        Vector2 dir = (clickPoint - (Vector2)playerTransform.position).normalized;
        player.FaceDirection(dir);
    }
    
    private void TryStartHoeing()
    {
        if (targetSoil == null) return;

        if (Vector2.Distance(playerTransform.position, targetSoil.transform.position) <= interactRange)
        {
            player.StartHoeing(); 
        }
    }
    
    public void OnHoeStage(int stage)
    {
        if (targetSoil && targetSoil.TryGetComponent(out GroundTile tile))
        {
            switch (stage)
            {
                case 1:
                    tile.ChangeToStage1();
                    break;
                case 2:
                    tile.ChangeToStage2();
                    break;
                case 3:
                    tile.ChangeToStage3();
                    break;
            }

            Debug.Log($"🪓 Hoe stage {stage} triggered!");
        }
    }
}
