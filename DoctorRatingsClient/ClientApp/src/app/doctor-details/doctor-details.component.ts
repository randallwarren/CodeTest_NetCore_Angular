import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-doctor-details',
  templateUrl: './doctor-details.component.html',
  styleUrls: ['./doctor-details.component.css']
})
export class DoctorDataComponent {
  public doctorDetails: DoctorDetailRow[];
  public patientReviews: PatientReviewRow[];

  constructor(
    http: HttpClient,
    @Inject('SERVER_URL') baseUrl: string,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private location: Location
  ){
//    console.log(activatedRoute.snapshot.url); // array of states
//    console.log(activatedRoute.snapshot.url[1].path); 

    http.get<DoctorDetailRow[]>(baseUrl + '/api/doctors/' + activatedRoute.snapshot.url[1].path).subscribe(result => {
      this.doctorDetails = result;
    }, error => console.error(error));

    http.get<PatientReviewRow[]>(baseUrl + '/api/reviews/' + activatedRoute.snapshot.url[1].path).subscribe(result => {
      this.patientReviews = result;
    }, error => console.error(error));
  }

  goBack(): void {
    this.location.back();
  }
}

interface DoctorDetailRow {
  id: number;
  name: string;
  gender: string;
  language: string;
  school: string;
  specialties: string;
  avgRating: number;
  superStar: string;
}

interface PatientReviewRow {
  id: number;
  doctorid: string;
  comments: string;
  rating: string;
}
