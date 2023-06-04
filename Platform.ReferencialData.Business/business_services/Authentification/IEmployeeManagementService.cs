using login.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferencialData.Business.business_services.Authentification
{
    public interface IEmployeeManagementService
    {
        Task<bool> AddRole(RoleRequestDto role);
        Task<DataResult> addEmployment(EmployDto employ);
        Task<bool> addClaims(AspRoles existing_Role, List<string> permissions);

        Task<List<Object>> getEmployementById(string id);





        Task addDemandeTransaction(demandeTransactionDto transaction);
        bool deleteDemandeTransaction(string id);

        Task<List<Object>> getDemandeTransaction(); 
        Task<List<demandeTransaction>> getDemandeTransactionByEmployer(string Id);
        bool updateDemandeTransaction(string id, string value);




        Task<List<demandePayement>> getDemandePayementByShopowner(string Id);
        List<demandePayement> getDemandePayementById(string Id);

        Task<List<Object>> getDemandePayement();
        Task addDemandePayement(demandePayementDto transaction);
        Task<bool> updateDemandePayement(string id, string value,string idvalidateur);




        Task<List<employer3>> getaccountName(List<string> list);


        Task<DataResult> updateEmployement(UpdateEmployementDto employ);

        Task FireEmployee(deleteEmploymentDto employ);

        Task<IList<string>> getRolesByUserId(string id);

        Task<bool> addTransaction(TransactionDto transaction);


        Task<bool> addPayement(PayementDto payement);



        string getAccount(string id);
        bool addaccount(string secret , string iduser);
    }
}
