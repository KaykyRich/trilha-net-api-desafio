using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {            
            var entidade = _context.Entidades.Find(id);
            if (entidade == null)
            {
                return NotFound();
            }
            return Ok(entidade);    
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var entidades = _context.Entidades.ToList();
            if (entidades == null)
            {
                return NotFound();
            }
            return Ok(entidades);
        }


        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var entidades = _context.Entidades.Where(e => e.Titulo.Contains(titulo)).ToList();
            if (entidades == null)
            {
                return NotFound();
            }
            return Ok(entidades);
        }


        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var entidades = _context.Entidades.Where(e => e.Status == status).ToList();
            if (entidades == null)
            {
                return NotFound();
            }
            return Ok(entidades);
        }


        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            _context.Tarefas.Add(tarefa);
    
            try
            {
                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Erro = "Ocorreu um erro ao criar a tarefa: " + ex.Message });
            }
        
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }


        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            // Busca a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);
        
            // Verifica se a tarefa foi encontrada
            if (tarefaBanco == null)
            {
                return NotFound();
            }
        
            // Verifica se a data da tarefa é válida
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }
        
            // Atualiza as informações da tarefa do banco com os dados recebidos por parâmetro
            tarefaBanco.Propriedade1 = tarefa.Propriedade1;
            tarefaBanco.Propriedade2 = tarefa.Propriedade2;
            // Adicione as demais propriedades que deseja atualizar
        
            try
            {
                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Em caso de erro ao salvar, retorna um StatusCode 500 (Internal Server Error) com a mensagem de erro
                return StatusCode(500, new { Erro = "Ocorreu um erro ao atualizar a tarefa: " + ex.Message });
            }
        
            // Retorna Ok
            return Ok();
        }


        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            // Busca a tarefa no banco de dados pelo ID
            var tarefaBanco = _context.Tarefas.Find(id);
        
            // Verifica se a tarefa foi encontrada
            if (tarefaBanco == null)
            {
                return NotFound();
            }
        
            // Remove a tarefa do contexto do Entity Framework
            _context.Tarefas.Remove(tarefaBanco);
        
            try
            {
                // Salva as mudanças no banco de dados
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Em caso de erro ao salvar, retorna um StatusCode 500 (Internal Server Error) com a mensagem de erro
                return StatusCode(500, new { Erro = "Ocorreu um erro ao deletar a tarefa: " + ex.Message });
            }
        
            // Retorna NoContent
            return NoContent();
        }

    }
}
