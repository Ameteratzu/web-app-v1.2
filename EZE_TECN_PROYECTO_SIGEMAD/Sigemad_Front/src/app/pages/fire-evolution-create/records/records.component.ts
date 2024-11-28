import { Component, EventEmitter, inject, OnInit, Output, signal } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { InputOutputService } from '../../../services/input-output.service';
import { MediaService } from '../../../services/media.service';
import { OriginDestinationService } from '../../../services/origin-destination.service';
import { FireStatusService } from '../../../services/fire-status.service';
import { EvolutionService } from '../../../services/evolution.service';
import { InputOutput } from '../../../types/input-output.type';
import { Media } from '../../../types/media.type';
import { OriginDestination } from '../../../types/origin-destination.type';
import { FireStatus } from '../../../types/fire-status.type';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MatNativeDateModule,
  NativeDateAdapter,
} from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { FlexLayoutModule } from '@angular/flex-layout'; 
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import moment from 'moment';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';

const MY_DATE_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL', 
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: 'app-records',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    FlexLayoutModule,
    MatGridListModule,
    MatButtonModule,
    MatButtonModule,
    MatSort,
    MatTableModule,
    MatIconModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    MatSelectModule
  ],
  templateUrl: './records.component.html',
  styleUrl: './records.component.scss',
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
  ],
})
export class RecordsComponent implements OnInit {

  @Output() save = new EventEmitter<void>();
  private fb = inject(FormBuilder); 
  data = inject(MAT_DIALOG_DATA) as { title: string };
  public matDialog = inject(MatDialog);
  public inputOutputService = inject(InputOutputService);
  public mediaService = inject(MediaService);
  public originDestinationService = inject(OriginDestinationService);
  public fireStatusService = inject(FireStatusService);
  public evolutionSevice = inject(EvolutionService);
  private spinner = inject(NgxSpinnerService);
  public toast = inject(MatSnackBar);
  
  formData!: FormGroup;
  public inputOutputs = signal<InputOutput[]>([]);
  public medias = signal<Media[]>([]);
  public originDestinations = signal<OriginDestination[]>([]);
  public status = signal<FireStatus[]>([]);
  public isCreate = signal<number>(-1);

  async ngOnInit() {
    const inputOutputs = await this.inputOutputService.get();
    this.inputOutputs.set(inputOutputs);

    const medias = await this.mediaService.get();
    this.medias.set(medias);

    const originDestinations = await this.originDestinationService.get();
    this.originDestinations.set(originDestinations);

    const status = await this.fireStatusService.get();
    this.status.set(status);

    

    this.formData = this.fb.group({
      inputOutput : ['', Validators.required],
      startDate: [new Date(), Validators.required],
      media: ['', Validators.required],
      originDestination: ['', Validators.required],
      datetimeUpdate: ['', Validators.required],
      observations_1: ['', Validators.required],
      forecast: ['', Validators.required],
      status: ['', Validators.required],
      end_date: [new Date(), Validators.required],
      emergencyPlanActivated: ['', Validators.required],

      
    });
  }

  async sendDataToEndpoint() {
    if (this.evolutionSevice.dataRecords().length > 0) {
      this.save.emit(); 
    }else{
      this.spinner.show();
      this.showToast();
    }
  }

  getFormatdate(date: any){
    return moment(date).format('DD/MM/YY')
  }

  showToast() {
    this.toast.open('Guardado correctamente', 'Cerrar', {
      duration: 3000, 
      horizontalPosition: 'right', 
      verticalPosition: 'top', 
    });
  }

  getForm(atributo: string): any {
    return this.formData.controls[atributo];
  }

  closeModal(){
    this.matDialog.closeAll();
  }

}
