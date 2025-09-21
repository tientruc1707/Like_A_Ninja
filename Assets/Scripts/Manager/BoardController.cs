using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public event Action OnMoveEvent = delegate { };
    public bool IsBusy { get; private set; }
    private Board m_board;
    private bool m_isDragging;
    private Camera m_cam;
    private Collider2D m_hitCollider;
    private GameSetting m_gameSetting;
    private List<Cell> m_potentialMatch;
    private float m_timeAfterFill;
    private bool m_hintIsShown;
    private bool m_gameOver;


    public void StartGame(GameSetting gameSetting)
    {
        m_gameSetting = Resources.Load<GameSetting>(StringConstant.GAME_SETTING_PATH);

        m_cam = Camera.main;

        m_board = new Board(this.transform, gameSetting);

        Fill();
    }

    private void Fill()
    {
        m_board.FillNewBoard();
        FindMatchesAndCollapse();
    }

    public void Update()
    {
        if (m_gameOver) return;
        if (IsBusy) return;

        if (!m_hintIsShown)
        {
            m_timeAfterFill += Time.deltaTime;
            if (m_timeAfterFill > m_gameSetting.TimeForHint)
            {
                m_timeAfterFill = 0f;
                ShowHint();
            }
        }
        if (GameManager.Instance.CurrentSide == TurnSide.LEFTTURN)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    m_isDragging = true;
                    m_hitCollider = hit.collider;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                ResetRayCast();
            }

            if (Input.GetMouseButton(0) && m_isDragging)
            {
                var hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    if (m_hitCollider != null && m_hitCollider != hit.collider)
                    {
                        StopHints();

                        Cell c1 = m_hitCollider.GetComponent<Cell>();
                        Cell c2 = hit.collider.GetComponent<Cell>();

                        if (AreItemsNeighbor(c1, c2))
                        {
                            IsBusy = true;
                            SetSortingLayer(c1, c2);
                            m_board.Swap(c1, c2, () =>
                            {
                                FindMatchesAndCollapse(c1, c2);
                            });

                            ResetRayCast();
                        }
                    }
                }
                else
                {
                    ResetRayCast();
                }

            }

        }
        else
        {
            AutoPlay();
        }

    }

    private void AutoPlay()
    {
        if (m_potentialMatch.Count > 0 && m_hintIsShown)
        {
            Cell cell1 = m_potentialMatch[1];
            Cell cell2 = m_potentialMatch[3];
            StopHints();
            IsBusy = true;
            SetSortingLayer(cell1, cell2);
            m_board.Swap(cell1, cell2, () =>
            {
                FindMatchesAndCollapse(cell1, cell2);
            });
            ResetRayCast();
        }
    }

    private void ResetRayCast()
    {
        m_isDragging = false;
        m_hitCollider = null;
    }

    private void FindMatchesAndCollapse(Cell cell1, Cell cell2)
    {
        if (cell1.Item is SpecialItem)
        {
            cell1.DestroyItem();
            StartCoroutine(ShiftDownItemsCoroutine());
        }
        else if (cell2.Item is SpecialItem)
        {
            cell2.DestroyItem();
            StartCoroutine(ShiftDownItemsCoroutine());
        }
        else
        {
            List<Cell> cells1 = GetMatches(cell1);
            List<Cell> cells2 = GetMatches(cell2);

            List<Cell> matches = new List<Cell>();
            matches.AddRange(cells1);
            matches.AddRange(cells2);
            matches = matches.Distinct().ToList();

            if (matches.Count < m_gameSetting.MatchMin)
            {
                m_board.Swap(cell1, cell2, () =>
                {
                    IsBusy = false;
                });
            }
            else
            {
                OnMoveEvent();

                CollapseMatches(matches, cell2);
            }
        }
    }

    private void FindMatchesAndCollapse()
    {
        List<Cell> matches = m_board.FindFirstMatch();

        if (matches.Count > 0)
        {
            CollapseMatches(matches, null);
        }
        else
        {
            m_potentialMatch = m_board.GetPotentialMatches();
            if (m_potentialMatch.Count > 0)
            {
                IsBusy = false;

                m_timeAfterFill = 0f;
            }
            else
            {
                //StartCoroutine(RefillBoardCoroutine());
                StartCoroutine(ShuffleBoardCoroutine());
            }

            EventSystem.Instance.TriggerEvent(StringConstant.EVENT.CHANG_SIDE);
        }
    }

    private List<Cell> GetMatches(Cell cell)
    {
        List<Cell> listHor = m_board.GetHorizontalMatches(cell);
        if (listHor.Count < m_gameSetting.MatchMin)
            listHor.Clear();

        List<Cell> listVer = m_board.GetVerticalMatches(cell);
        if (listVer.Count < m_gameSetting.MatchMin)
            listVer.Clear();

        return listHor.Concat(listVer).Distinct().ToList();
    }

    private void CollapseMatches(List<Cell> matches, Cell cellEnd)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            matches[i].DestroyItem();
        }

        if (matches.Count > m_gameSetting.MatchMin)
        {
            m_board.ConvertNormalToSpecial(matches, cellEnd);
        }

        StartCoroutine(ShiftDownItemsCoroutine());
    }

    private IEnumerator ShiftDownItemsCoroutine()
    {
        m_board.ShiftDownItems();

        yield return new WaitForSeconds(0.2f);

        m_board.FillGapsWithNewItems();

        yield return new WaitForSeconds(0.2f);

        FindMatchesAndCollapse();
    }

    private IEnumerator RefillBoardCoroutine()
    {
        m_board.ClearBoard();

        yield return new WaitForSeconds(0.2f);

        m_board.FillNewBoard();

        yield return new WaitForSeconds(0.2f);

        FindMatchesAndCollapse();
    }

    private IEnumerator ShuffleBoardCoroutine()
    {
        m_board.Shuffle();

        yield return new WaitForSeconds(0.3f);

        FindMatchesAndCollapse();
    }

    private void SetSortingLayer(Cell cell1, Cell cell2)
    {
        if (cell1.Item != null) cell1.Item.SetSortingLayerHigher();
        if (cell2.Item != null) cell2.Item.SetSortingLayerLower();
    }

    private bool AreItemsNeighbor(Cell cell1, Cell cell2)
    {
        return cell1.IsNeighbour(cell2);
    }

    internal void Clear()
    {
        m_board.Clear();
    }

    private void ShowHint()
    {
        m_hintIsShown = true;
        foreach (var cell in m_potentialMatch)
        {
            cell.AnimationForHint();
        }
    }

    private void StopHints()
    {
        m_hintIsShown = false;
        foreach (var cell in m_potentialMatch)
        {
            cell.StopAnimationForHint();
        }

        m_potentialMatch.Clear();
    }

}
