import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public doctors: DoctorRow[];

  constructor(http: HttpClient, @Inject('SERVER_URL') baseUrl: string) {
    http.get<DoctorRow[]>(baseUrl + '/api/doctors').subscribe(result => {
      this.doctors = result;
    }, error => console.error(error));
  }
}

interface DoctorRow {
  id: number;
  name: string;
  gender: string;
  language: string;
  school: string;
  specialties: string;
  avgRating: number;
  superStar: string;
}
