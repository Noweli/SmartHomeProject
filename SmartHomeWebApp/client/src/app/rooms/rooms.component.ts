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

  async ngOnInit(): Promise<void> {
    await this.getRooms();
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
          element.editMode = false;
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

  toggleEditMode(room: any): void {
    room.editMode = !room.editMode;
  }

  cancelEditMode(event: any): void {
    let room = this.rooms.find((r: any) => r.id = event.id);
    const index = this.rooms.indexOf(room);
    room = event;
    this.rooms[index] = room;
    location.reload();
  }

  toggleHeater(room: any): void{
    const headers = new HttpHeaders().set(
      'Authorization',
      'Bearer ' + this.currentUser.token
    );

    // tslint:disable-next-line: max-line-length
    this.http.get(`${this.baseUrl}request/heater/${room.heaterEnabled ? 'off' : 'on'}/${room.name}`, {headers, responseType: 'text'}).subscribe(() => {
      location.reload();
    }, err =>{
      console.warn(err);
    });
  }

  getEnableHeaterButtonText(room: any): string{
    if (room.heaterEnabled){
      return 'Turn off heater';
    }

    return 'Turn on heater';
  }

  getAutoHeaterButtonText(room: any): string{
    if(room.autoHeatEnabled){
      return 'Turn off auto heat';
    }

    return 'Turn on auto heat';
  }
}
