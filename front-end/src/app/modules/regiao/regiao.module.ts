import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegiaoComponent } from './regiao.component';
import { RegiaoRoutingModule } from './regiao.routing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RegiaoFormComponent } from './regiao-form/regiao-form.component';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RegiaoRoutingModule
  ],
  declarations: [RegiaoComponent, RegiaoFormComponent],
  exports: [RegiaoComponent]
})
export class RegiaoModule { }