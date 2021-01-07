import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css'],
})
export class RoomsComponent implements OnInit {
  currentUser: any;
  rooms: any = {};
  model: any = {};
  addingNewRoom = false;
  baseUrl = 'http://localhost:5000/api/';

  constructor(public accountService: AccountService, private http: HttpClient) {
    accountService.currentUser$.subscribe((user) => (this.currentUser = user));
  }

  ngOnInit(): void {
    this.getRooms();
  }

  async getRooms(): Promise<void> {
    const headers = new HttpHeaders().set(
      'Authorization',
      'Bearer ' + this.currentUser.token
    );

    this.http.get(this.baseUrl + 'room/getRooms', { headers }).subscribe(
      (response) => {
        this.rooms = response;

        this.rooms.forEach(async (element: any) => {
          await this.getRoomTemperature(element.name).then((result: any) => {
            element.temperature = result.temperature;
            element.humidity = result.humidity;
          });
          console.log(element);
        });
      },
      (err) => {
        console.log(err);
      }
    );
  }

  async getRoomTemperature(roomName: string): Promise<object> {
    const headers = new HttpHeaders().set(
      'Authorization',
      'Bearer ' + this.currentUser.token
    );

    return this.http
      .get(this.baseUrl + 'request/sensor/temperature/' + roomName, { headers })
      .toPromise();
  }

  toggleAutoHeater(room: any): void{
    const headers = new HttpHeaders().set(
      'Authorization',
      'Bearer ' + this.currentUser.token
    );

    // tslint:disable-next-line: max-line-length
    this.http.get(`${this.baseUrl}room/enableAutoHeater?roomName=${room.name}&enabled=${!room.autoHeatEnabled}`, {headers}).subscribe(response => {
      location.reload();
    });
  }

  cancelAddingMode(event: boolean): void {
    this.addingNewRoom = event;
    location.reload();
  }

  toggleAddingMode(): void{
    this.addingNewRoom = !this.addingNewRoom;
  }

  toggleHeater(room: any, action: boolean): void{
    const headers = new HttpHeaders().set(
      'Authorization',
      'Bearer ' + this.currentUser.token
    );

    this.http.get(`${this.baseUrl}request/heater/${action ? 'on' : 'off'}/${room.name}`, {headers}).subscribe(response => {
      location.reload();
    });
  }

  getHeaterButtonText(room: any): string{
    if(room.autoHeatEnabled){
      return 'Turn off auto heat';
    }

    return 'Turn on auto heat';
  }
}
