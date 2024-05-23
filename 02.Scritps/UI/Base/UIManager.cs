using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��� UI�� ����, ��ü��ũ���� �� �˾��� UI �߰��� ����.
/// </summary>
public class UIManager : SingletonMonoBase<UIManager>
{
    private Dictionary<Type, IUI> _uis = new Dictionary<Type, IUI>(); // Ÿ������ UI �˻�
    private List<IUI> _screens = new List<IUI>(); // Ȱ��ȭ �Ǿ��ִ� Screen UI ��
    private Stack<IUI> _popups = new Stack<IUI>(); // Ȱ��ȭ �Ǿ��ִ� Popup UI ��


    private void Update()
    {
        UpdateInpuActions();
    }

    /// <summary>
    /// ���� ��ȣ�ۿ� ������ UI �� ��ȣ�ۿ� ������Ʈ
    /// </summary>
    public void UpdateInpuActions()
    {
        // Ȱ��ȭ�� Popup�� �����Ѵٸ� �ֻ�� Popup UI �� ��ȣ�ۿ�
        if (_popups.Count > 0)
        {
            if (_popups.Peek().inputActionEnable)
                _popups.Peek().InputAction();
        }

        // Ȱ��ȭ�� Screen UI �� �����Ѵٸ� ��� ��ȣ�ۿ�
        for (int i = _screens.Count - 1; i >= 0; i--)
        {
            if (_screens[i].inputActionEnable)
                _screens[i].InputAction();
        }
    }

    /// <summary>
    /// ��ũ���� UI ���
    /// </summary>
    public void RegisterScreen(IUI ui)
    {
        if (_uis.TryAdd(ui.GetType(), ui))
        {
        }
        else
        {
            throw new Exception($"[UIManager] : UI ��� ����. {ui.GetType()} �� �̹� ��ϵǾ��ֽ��ϴ�...");
        }
    }

    /// <summary>
    /// �˾��� UI ���
    /// </summary>
    public void RegisterPopup(IUI ui)
    {
        if (_uis.TryAdd(ui.GetType(), ui))
        {
        }
        else
        {
            throw new Exception($"[UIManager] : UI ��� ����. {ui.GetType()} �� �̹� ��ϵǾ��ֽ��ϴ�...");
        }
    }

    /// <summary>
    /// Resolve. ���ϴ� UI �������� �Լ�
    /// </summary>
    /// <typeparam name="T"> �������� ���� UI Ÿ�� Type ��ü </typeparam>
    public T Get<T>()
        where T : IUI
    {
        return (T)_uis[typeof(T)];
    }

    /// <summary>
    /// ���� Ȱ��ȭ�� Popup UI ����
    /// </summary>
    public void PushPopup(IUI ui)
    {
        if (_popups.Count > 0)
        {
            //_popups.Peek().Hide();
            _popups.Peek().inputActionEnable = false; // ������ �ִ� �˾��� �Է� �ȸ�����
        }
            

        _popups.Push(ui);
        _popups.Peek().inputActionEnable = true; // ���� ��� �˾��� �Է� ������
        _popups.Peek().sortingOrder = _popups.Count; // ���� ��� �˾��� �ֻ������ ����
    }

    /// <summary>
    /// �������� Popup UI ����
    /// </summary>
    /// <exception cref="Exception"> Popup UI�� �ֻ�� ���͸� ���� �� ����. �������� UI �� �ֻ�ܿ� ������� </exception>
    public void PopPopup(IUI ui)
    {
        // �������� UI �� �ֻ�ܿ� ���� ������ ����
        if (_popups.Peek() != ui)
            throw new Exception($"[UIManager] : {ui.GetType()} �˾��� �ݱ� �õ������� �ֻ�ܿ� ��������..");

        _popups.Pop();

        // ���� �˾��� �Է��� ������
        if (_popups.Count > 0)
            _popups.Peek().inputActionEnable = true;
    }

    /// <summary>
    /// ��üȭ�� UI �� ����
    /// </summary>
    public void SetScreen(IUI ui)
    {
        for (int i = _screens.Count - 1; i >= 0; i--)
        {
            _screens[i].Hide();
            _screens.RemoveAt(i);
        }

        _screens.Add(ui);
        ui.inputActionEnable = true;
    }
}