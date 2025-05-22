// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SlotItemsSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    ///   Adds items under specified fixed slots.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Slot Items Setter")]
    public class SlotItemsSetter : GameObjectItemsSetter
    {
        /// <summary>
        ///   Indicates if slots that don't hold an item should be hidden.
        /// </summary>
        [Tooltip("Indicates if slots that don't hold an item should be hidden.")]
        public bool HideEmptySlots;

        /// <summary>
        ///   Indicates if items are shifted to higher slots when an item in the middle of the collection is removed.
        /// </summary>
        [Tooltip(
            "Indicates if items are shifted to higher slots when an item in the middle of the collection is removed.")]
        public bool ShiftItemsOnRemove;

        /// <summary>
        ///   Event when an item of a slot was destroyed.
        /// </summary>
        public SlotItemDestroyedEvent SlotItemDestroyed;

        /// <summary>
        ///   Slots to add items to.
        /// </summary>
        public Transform[] Slots;

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();

            if (this.HideEmptySlots)
            {
                // Hide slots.
                foreach (var slot in this.Slots)
                {
                    slot.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        ///   Called when an item for an item context was created.
        /// </summary>
        /// <param name="itemContext">Item context the item is for.</param>
        /// <param name="itemObject">Item game object.</param>
        /// <param name="itemIndex"></param>
        protected override void OnItemCreated(object itemContext, GameObject itemObject, int itemIndex)
        {
            // Get empty slot.
            var itemSlot = this.GetEmptySlot();
            if (itemSlot == null)
            {
                Debug.LogWarning("No empty slot remaining, hiding item object for now.", this);
                itemObject.SetActive(false);
                itemObject.transform.SetParent(this.transform, false);
            }
            else
            {
                this.SetSlotItem(itemSlot, itemObject);
            }
        }

        /// <summary>
        ///   Called when an item for an item context was destroyed.
        /// </summary>
        /// <param name="itemContext">Item context the item was for.</param>
        /// <param name="itemObject">Item game object.</param>
        protected override void OnItemDestroyed(object itemContext, GameObject itemObject)
        {
            var itemSlot = itemObject.transform.parent;
            if (itemSlot == null)
            {
                return;
            }

            var itemSlotIndex = Array.IndexOf(this.Slots, itemSlot);
            this.SlotItemDestroyed.Invoke(new SlotItemDestroyedEventData { SlotIndex = itemSlotIndex });

            if (this.ShiftItemsOnRemove)
            {
                // Reparent items in lower slots.
                if (itemSlotIndex >= 0)
                {
                    var itemGameObjects = this.ItemGameObjects.ToList();
                    for (var slotIndex = itemSlotIndex; slotIndex < this.Slots.Length; slotIndex++)
                    {
                        var slot = this.Slots[slotIndex];
                        var itemGameObject = slotIndex < itemGameObjects.Count ? itemGameObjects[slotIndex] : null;
                        if (itemGameObject != null)
                        {
                            this.SetSlotItem(slot, itemGameObject);

                            // Activate in case item object was unassigned and hidden before.
                            itemGameObject.SetActive(true);
                        }
                        else
                        {
                            this.ClearSlot(slot);
                        }
                    }
                }
            }
            else
            {
                this.ClearSlot(itemSlot);
            }
        }

        private void ClearSlot(Transform slot)
        {
            if (this.HideEmptySlots)
            {
                // Deactivate parent of item.
                slot.gameObject.SetActive(false);
            }
        }

        private Transform GetEmptySlot()
        {
            var itemGameObjects = this.ItemGameObjects.ToList();

            // Get first slot where no item was placed under.
            return
                this.Slots.FirstOrDefault(
                    slot => !itemGameObjects.Any(itemGameObject => itemGameObject.transform.parent == slot));
        }

        private void SetSlotItem(Transform slot, GameObject itemGameObject)
        {
            if (this.HideEmptySlots)
            {
                // Activate parent of item.
                slot.gameObject.SetActive(true);
            }

            itemGameObject.transform.SetParent(slot, false);
        }

        /// <summary>
        ///   Event which is fired when a slot item was destroyed.
        /// </summary>
        [Serializable]
        public class SlotItemDestroyedEvent : UnityEvent<SlotItemDestroyedEventData>
        {
        }

        /// <summary>
        ///   Event data for the <see cref="SlotItemDestroyedEvent" />.
        /// </summary>
        public class SlotItemDestroyedEventData
        {
            /// <summary>
            ///   Index of slot that was destroyed.
            /// </summary>
            public int SlotIndex { get; set; }
        }
    }
}