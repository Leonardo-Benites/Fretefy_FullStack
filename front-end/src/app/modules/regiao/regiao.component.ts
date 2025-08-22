import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Regiao } from 'src/app/interfaces/regiao';
import { RegiaoService } from 'src/app/services/regiao.service';
import Swal from 'sweetalert2'

@Component({
  selector: 'app-regiao',
  templateUrl: './regiao.component.html',
  styleUrls: ['./regiao.component.scss']
})
export class RegiaoComponent implements OnInit {

 regioes: Observable<Regiao[]>;
  loading = true;
  exportando = false;

  constructor(
    private regiaoService: RegiaoService,
    private router: Router
  ) {
    this.regioes = new Observable<Regiao[]>();
  }

  ngOnInit() {
    this.carregarRegioes();
  }

   private carregarRegioes() {
    this.regioes = this.regiaoService.listarRegioes()
    this.regioes.subscribe({
      next: () => {
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;
        Swal.fire({
          icon: 'error',
          title: 'Erro ao carregar regiões',
          text: 'Não foi possível carregar a lista de regiões. Tente novamente.',
          confirmButtonColor: '#3b82f6'
        });
        console.error('Erro ao carregar regiões:', error);
      }
    });
  }

   novaRegiao() {
    this.router.navigate(['/regiao/nova']);
  }

  editarRegiao(id: number) {
    this.router.navigate(['/regiao/editar', id]);
  }

  async alternarStatus(regiao: Regiao) {
    const novoStatus = { id: regiao.id, ativo: !regiao.ativo };
    const acao = novoStatus.ativo ? 'ativar' : 'desativar';
    console.log(`Tentando ${acao} região:`, regiao);

    const result = await Swal.fire({
      title: `Deseja realmente ${acao} esta região?`,
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: novoStatus.ativo ? '#f59e0b' : '#10b981',
      cancelButtonColor: '#6b7280',
      confirmButtonText: `Sim, ${acao}`,
      cancelButtonText: 'Cancelar'
    });

    if (result.isConfirmed) {
      this.regiaoService.alterarStatus(regiao.id, novoStatus).subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Sucesso!',
            text: `Região ${acao === 'desativar' ? 'desativada' : 'ativada'} com sucesso!`,
            timer: 2000,
            showConfirmButton: false
          });
          this.carregarRegioes();
        },
        error: (error) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: `Erro ao ${acao} região. Tente novamente.`,
          });
          console.error(`Erro ao ${acao} região:`, error);
        }
      });
    }
  }

  exportarExcel() {
    this.exportando = true;
    this.regiaoService.exportarExcel().subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'regioes.xlsx';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
        this.exportando = false;
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Erro ao exportar',
          text: 'Não foi possível exportar as regiões. Tente novamente.',
          confirmButtonColor: '#3b82f6'
        });
        console.error('Erro ao exportar regiões:', error);
        this.exportando = false;
      }
    });
  }
}
