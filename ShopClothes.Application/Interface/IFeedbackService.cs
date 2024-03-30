using ShopClothes.Application.ViewModel.Common;
using ShopClothes.Infastructure.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Application.Interface
{
    public interface IFeedbackService
    {
        void Add(FeedbackViewModel feedbackVm);

        void Update(FeedbackViewModel feedbackVm);

        void Delete(int id);

        List<FeedbackViewModel> GetAll();

        PagedResult<FeedbackViewModel> GetAllPaging(string keyword, int page, int pageSize);

        FeedbackViewModel GetById(int id);

        void SaveChanges();
    }
}
