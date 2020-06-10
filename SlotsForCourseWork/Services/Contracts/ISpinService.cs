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
        ResultDTO StartUser(SpinViewModel model, User user);
        ResultDTO StartGuest(SpinViewModel model);
    }
}