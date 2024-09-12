using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Player : Unit
{
    [SerializeField]
    private int _maxEquippedCount = 2;
    [SerializeField]
    private List<Unit> _equippedUnitList = new();
    [SerializeField]
    private List<Unit> _inventoryUnitList = new();

    public Player() : base(0)
    {

    }

    /// <summary>
    /// 获得手持中的单位。
    /// </summary>
    /// <param name="index">手持位置</param>
    /// <returns>单位</returns>
    public Unit GetEquippedUnit(int index) => index >= 0 && index < _equippedUnitList.Count ? _equippedUnitList[index] : null;

    /// <summary>
    /// 获得背包中的单位。
    /// </summary>
    /// <param name="index">背包位置</param>
    /// <returns>单位</returns>
    public Unit GetInventoryUnit(int index) => index >= 0 && index < _inventoryUnitList.Count ? _inventoryUnitList[index] : null;

    /// <summary>
    /// 装备单位。
    /// </summary>
    /// <param name="unit">单位</param>
    /// <param name="index">手持位置</param>
    public void EquipUnit(Unit unit, int index)
    {
        if (index < 0 || index >= _maxEquippedCount) return;
        while (index >= _equippedUnitList.Count) _equippedUnitList.Add(null);
        if (_equippedUnitList[index] == unit) return;
        if (_equippedUnitList[index] != null && !_inventoryUnitList.Contains(_equippedUnitList[index])) _inventoryUnitList.Add(_equippedUnitList[index]);
        _inventoryUnitList.Remove(unit);
        if (_equippedUnitList.Contains(unit)) _equippedUnitList[_equippedUnitList.IndexOf(unit)] = null;
        _equippedUnitList[index] = unit;
        TriggerDataChanged(this);
    }

    /// <summary>
    /// 卸载单位。
    /// </summary>
    /// <param name="unit">单位</param>
    public void UnequipUnit(Unit unit)
    {
        if (_equippedUnitList.Contains(unit))
        {
            _inventoryUnitList.Add(unit);
            _equippedUnitList[_equippedUnitList.IndexOf(unit)] = null;
            TriggerDataChanged(this);
        }
    }

    /// <summary>
    /// 卸载单位。
    /// </summary>
    /// <param name="index">手持位置</param>
    public void UnequipUnit(int index)
    {
        if (index >= 0 && index < _equippedUnitList.Count)
        {
            _inventoryUnitList.Add(_equippedUnitList[index]);
            _equippedUnitList[index] = null;
            TriggerDataChanged(this);
        }
    }

    /// <summary>
    /// 将单位添加到背包。
    /// </summary>
    /// <param name="unit">单位</param>
    public void GainUnit(Unit unit)
    {
        if (!_inventoryUnitList.Contains(unit))
        {
            if (_equippedUnitList.Contains(unit)) _equippedUnitList[_equippedUnitList.IndexOf(unit)] = null;
            _inventoryUnitList.Add(unit);
            TriggerDataChanged(this);
        }
    }

    /// <summary>
    /// 将单位移除背包。
    /// </summary>
    /// <param name="unit">单位</param>
    public void LostUnit(Unit unit)
    {
        if (_inventoryUnitList.Remove(unit)) TriggerDataChanged(this);
    }

    /// <summary>
    /// 手持中的单位数量。
    /// </summary>
    public int EquippedCount => _equippedUnitList.Count;

    /// <summary>
    /// 背包中的单位数量。
    /// </summary>
    public int InventoryCount => _inventoryUnitList.Count;
}
