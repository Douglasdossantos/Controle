using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Controle.Models;
using Controle.Extended;

namespace Controle.Controllers
{
    public class TimesController : Controller
    {
        private ContextoDB db = new ContextoDB();

        public ActionResult Entrar()
        {
            var usuarioTarefasModel = new UsuarioTarefasModel
            {
                NomeConpleto = User.GetFullName(),
                Apelido = User.GetNickName(),
                email = User.GetEmail()
            };

            db.Usuarios.Add(usuarioTarefasModel);
            db.SaveChanges();

            return RedirectToAction("index");
        }
        public ActionResult Index()
        {
            var apelido = User.GetNickName();
            var times = db.Times.Where(w => w.Dono == apelido).ToList();
            return View(times);
        }
        public ActionResult Cadastro(int? Id)
        {
            var timeModel = new TimeModel();
            timeModel.Dono = User.GetNickName();
            if (Id != null || Id > 0 )
            {
                timeModel = db.Times.Find(Id);

                if (timeModel == null )
                {
                    return HttpNotFound();
                }
            }
            return View(timeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro([Bind(Include = "Id,Nome,Dono")] TimeModel timeModel)
        {
            if (ModelState.IsValid)
            {
                timeModel.Dono = User.GetNickName();
                if (timeModel.Id > 0)
                {
                    db.Entry(timeModel).State = EntityState.Modified;
                }
                else
                {
                    db.Times.Add(timeModel);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeModel);            
        }

        [HttpPost]
        public ActionResult Excluir(int Id)
        {
            var excluir = true;
            var msg = string.Empty;

            using (var transac = db.Database.BeginTransaction())
            {
                try
                {
                    var sqlExcluirTarefas = $@"DELETE FROM TarefaModel WHERE TimeModelId ={Id}";
                    var sqlExcluirUsuarios = $@"DELETE FROM UsuariosTimesModel WHERE TimeModelId ={Id}";

                    TimeModel timeModel = db.Times.Find(Id);
                    db.Times.Remove(timeModel);

                    db.Database.ExecuteSqlCommand(sqlExcluirTarefas);
                    db.Database.ExecuteSqlCommand(sqlExcluirUsuarios);

                    db.SaveChanges();

                    transac.Commit();

                }
                catch (Exception ex)
                {
                    msg = string.IsNullOrWhiteSpace(ex.Message)
                        ? ex.InnerException.Message
                        : ex.Message;
                    excluir = false;
                }

            }
            return View();
        }

        #region gerenciamento dos usuarios
        public ActionResult UsuarioTime(int Id)
        {
            var sql = $@"
	                    select u.* from UsuariosTimesModel Ut
				        JOIN UsuarioTarefasModel u ON ut.UsuariosTarefasModelId = u.Id
				        where Ut.TimeModelId = {Id}";
            var usuarios = db.Database.SqlQuery<UsuarioTarefasModel>(sql);
            ViewBag.Time = Id;
            return View(usuarios);
        }

        public ActionResult BuscarUsuarios(int Id)
        {
            ViewBag.Time = Id;
            return View();
        }

        public ActionResult PesquisarUsuario(string Apelido)
        {
            var dono = User.GetNickName();
            var sql = string.Format($@"SELECT * FROM UsuarioTarefasModel WHERE Apelido LIKE '%{Apelido}%' AND Apelido NOT LIKE '{dono}'");
            var usuarios = db.Database.SqlQuery<UsuarioTarefasModel>(sql).ToList();
            return Json(usuarios, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUsuarioTime(int Id, int IdUsuario)
        {
            var Gravou = true;
            var msg = string.Empty;
            if (Id > 0 && IdUsuario > 0)
            {
                var sql = string.Format($@"
                    IF EXISTS(SELECT * FROM UsuariosTimesModel where TimeModelId = {Id} and UsuariosTarefasModelId = {IdUsuario} )
                        BEGIN 
                            DELETE FROM  UsuariosTimesModel WHERE TimeModelId = {Id} AND UsuariosTarefasModelId = {IdUsuario}
                        END
                    else
                        BEGIN
                            INSERT INTO UsuariosTimesModel Values  ({IdUsuario}, {Id})
                        END  ");
                try
                {
                    db.Database.ExecuteSqlCommand(sql);
                }
                catch (Exception ex)
                {
                    msg = string.IsNullOrWhiteSpace(ex.Message)
                                 ? ex.InnerException.Message
                                 : ex.Message;
                    Gravou = false;
                }

                return Json(new { Gravou, msg });
            }
            return Json(new { Gravou = false, msg = "Parãmetros Incorretos!" });
        }
       

#endregion

    }
}
