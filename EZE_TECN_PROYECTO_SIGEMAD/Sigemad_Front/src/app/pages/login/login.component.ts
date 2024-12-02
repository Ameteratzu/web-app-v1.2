import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';

import { Router } from '@angular/router';

import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
  
@Component({
  selector: 'fire-create-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    FlexLayoutModule,
    MatCardModule,
    MatIconModule,
    NgxSpinnerModule
  ],
  providers: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class Login implements OnInit {
  public formData!: FormGroup;
  private spinner = inject(NgxSpinnerService);

  constructor(private authService: AuthService, private router: Router) {}

  async ngOnInit() {
    this.formData = new FormGroup({
      usuario: new FormControl('', Validators.required),
      clave: new FormControl('', Validators.required),
    });
  }

  async onSubmit() {
    this.formData.markAllAsTouched();
    if (this.formData.valid) {
      this.spinner.show()
      const data = this.formData.value;
      const body = {
        email: data.usuario,
        password: data.clave,
      };
      await this.authService
        .post(body)
        .then((response) => {
          //TODO toast
          console.info('response', response);
          new Promise((resolve) => setTimeout(resolve, 2000)).then(() => {
            this.spinner.hide()
            this.router.navigate([`/dashboard`])
          }
          );
        })
        .catch((error) => {
          this.formData.reset();
          alert(error.Message);
          console.log(error);
        });
    } else {
      this.formData.markAllAsTouched();
    }
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }
}
