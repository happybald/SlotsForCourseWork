using SlotsForCourseWork.Models;
using System.Linq;
using SlotsForCourseWork.ViewModels;
using System.Threading.Tasks;
using SlotsForCourseWork.DTO;
using Microsoft.AspNetCore.Mvc;

namespace SlotsForCourseWork.Services.Contracts
{
    public interface ISpinService
    {
        ResultDto StartUser(SpinViewModel model, User user);
        ResultDto StartGuest(SpinViewModel model);
    }
}