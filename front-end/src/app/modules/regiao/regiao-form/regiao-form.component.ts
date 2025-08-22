import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AtualizarRegiaoRequest } from 'src/app/interfaces/AtualizarRegiaoRequest';
import { CidadeService } from 'src/app/services/cidade.service';
import { RegiaoService } from 'src/app/services/regiao.service';



import Swal from 'sweetalert2'

@Component({
  selector: 'app-regiao-form',
  templateUrl: './regiao-form.component.html',
  styleUrls: ['./regiao-form.component.scss']
})
export class RegiaoFormComponent implements OnInit {
  id: string | null = null;
  regiaoForm: FormGroup;
  cidadesDisponiveis;

  constructor(
    private fb: FormBuilder, 
    private regiaoService: RegiaoService, 
    private cidadeService: CidadeService,
    private route: ActivatedRoute, 
    private router: Router
  ) { }

  ngOnInit(): void {
    this.regiaoForm = this.fb.group({
      nome: ['', Validators.required],
      ativo: [false],
      cidades: this.fb.array([])
    });


    this.route.paramMap.subscribe(params => {
      this.id = params.get('id');
    });

    this.cidadeService.listarCidades().subscribe(data => {
      this.cidadesDisponiveis = data;

      if (this.id) {
        this.regiaoService.buscarRegiaoPorId(this.id).subscribe((data: any) => {
          const regiao = data;

          this.regiaoForm.patchValue({
            nome: regiao.nome,
            ativo: regiao.ativo,
          })
          for (let i = 0; i < regiao.cidades.length; i++) {
            this.adicionarCidade(regiao.cidades[i])
          }
        });
      } else {
        this.adicionarCidade()
      }
    })
  }

  getCidades(): FormArray {
    return this.regiaoForm.get('cidades') as FormArray;
  }

  adicionarCidade(cidade = null): void {
    const cidadeFormGroup = this.fb.group({
      id: [cidade?.['id'] || '', Validators.required],
    });
    this.getCidades().push(cidadeFormGroup);
  }

  removerCidade(i) {
    this.getCidades().removeAt(i)
  }

  onSubmit() {
    if (this.regiaoForm.invalid) {
      Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: 'Preencha todos os campos corretamente.',
        confirmButtonColor: '#3b82f6'
      });

      return;
    }

    if (this.regiaoForm.value.cidades.length < 1) {
      Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: 'Selecione pelo menos uma cidade.',
        confirmButtonColor: '#3b82f6'
      });
      return;
    }
    const cidadesSelecionadas = this.regiaoForm.value.cidades;

    let idsCidades = cidadesSelecionadas.map(c => c.id);
    const temDuplicado = new Set(idsCidades).size !== idsCidades.length;

    if (temDuplicado) {
      Swal.fire({
        icon: 'error',
        title: 'Erro',
        text: 'Não é possível cadastrar cidades duplicadas.',
        confirmButtonColor: '#3b82f6'
      });
      return;
    }

    const cidadesIds: string[] = idsCidades
      .map(id => this.cidadesDisponiveis.find(cidade => cidade.id === id)?.id)
      .filter(id => !!id);

    const value = this.regiaoForm.value

    if (this.id) {
      let modelAtualizar: AtualizarRegiaoRequest = {
        id: this.id,
        nome: value.nome,
        ativo: value.ativo,
        cidades: cidadesIds
      }
      this.regiaoService.atualizarRegiao(modelAtualizar).subscribe(data => {
        Swal.fire({
          icon: 'success',
          title: 'Sucesso',
          text: 'Atualizado com sucesso!',
          confirmButtonColor: '#3b82f6'
        });
        this.regiaoForm.reset();
        this.getCidades().clear();
        this.adicionarCidade();
        this.router.navigate(['/regiao'])
      }, error => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: 'Ocorreu um erro',
          confirmButtonColor: '#3b82f6'
        });
      })
    } else {
      let model = {
        nome: value.nome,
        cidades: cidadesIds
      }
      this.regiaoService.criarRegiao(model).subscribe(data => {
        Swal.fire({
          icon: 'success',
          title: 'Sucesso',
          text: 'Salvo com sucesso!',
          confirmButtonColor: '#3b82f6'
        });
        this.regiaoForm.reset();
        this.getCidades().clear();
        this.adicionarCidade();
        this.router.navigate(['/regiao'])
      }, error => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: 'Ocorreu um erro',
          confirmButtonColor: '#3b82f6'
        });
      })
    }
  }
}