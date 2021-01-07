import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})

export class RoomsComponent implements OnInit {
  currentUser: any;
  rooms: any = {};
  model: any = {};
  baseUrl = 'http://localhost:5000/api/';

  constructor(public accountService: AccountService, private http: HttpClient) {
    accountService.currentUser$.subscribe(user => this.currentUser = user);
  }

  ngOnInit(): void {
    this.getRooms();
  }

  async getRooms(): Promise<void>{
    const headers = new HttpHeaders()
    .set('Authorization', 'Bearer ' + this.currentUser.token);

    this.http.get(this.baseUrl + 'room/getRooms', { headers }).subscribe(response => {
      this.rooms = response;

      this.rooms.forEach(async (element: any) => {
       await this.getRoomTemperature(element.name).then((result: any) => element.temperature = result.temperature);
        console.log(element);
      });
    }, (err) => {
      console.log(err);
    }
    );
  }

  async getRoomTemperature(roomName: string): Promise<object>{
    const headers = new HttpHeaders()
    .set('Authorization', 'Bearer ' + this.currentUser.token);

    return this.http.get(this.baseUrl + 'request/sensor/temperature/' + roomName, {headers}).toPromise();

  }
}
