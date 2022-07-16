using Microsoft.AspNetCore.Mvc.Rendering;

namespace Store.Models
{
    public class SortViewModel
    {
        public SortState NameSort { get; set; } // значение для сортировки по имени
        public SortState PriceSort { get; set; }   // значение для сортировки по компании
        public SortState TypeSort { get; set; }   // значение для сортировки по компании
        public SortState Current { get; set; }     // значение свойства, выбранного для сортировки
        public bool Up { get; set; }  // Сортировка по возрастанию или убыванию

        public SortViewModel(SortState sortOrder)
        {
            // значения по умолчанию
            NameSort = SortState.NameAsc;
            TypeSort = SortState.TypeAsc;
            Up = true;

            if (sortOrder == SortState.NameDesc
                || sortOrder == SortState.TypeDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    Current = NameSort = SortState.NameAsc;
                    break;
                case SortState.TypeAsc:
                    Current = TypeSort = SortState.TypeDesc;
                    break;
                case SortState.TypeDesc:
                    Current = TypeSort = SortState.TypeAsc;
                    break;
                case SortState.PriceAsc:
                    Current = PriceSort = SortState.PriceDesc;
                    break;
                case SortState.PriceDesc:
                    Current = PriceSort = SortState.PriceAsc;
                    break;
                default:
                    Current = NameSort = SortState.NameDesc;
                    break;
            }
        }
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public SelectList Types { get; set; } = new SelectList(new List<Type>(), "Id", "Name");
        public string? Name { get; set; }
    }
}
