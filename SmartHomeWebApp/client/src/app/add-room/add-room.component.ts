import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.css']
})
export class AddRoomComponent implements OnInit {
  @Output() cancelAdding = new EventEmitter();
  room: any = {};
  currentUser: any;
  baseUrl = 'http://localhost:5000/api/';

  constructor(public accountService: AccountService, private http: HttpClient) {
    accountService.currentUser$.subscribe((user: User) => (this.currentUser = user));
  }

  ngOnInit(): void {
  }

  addRoom(): void {
    const headers = new HttpHeaders().set(
      'Authorization',
      'Bearer ' + this.currentUser.token
    );

    this.http.post(this.baseUrl + 'room/add', this.room, {headers}).subscribe();
    this.cancel();
  }

  cancel(): void {
    this.cancelAdding.emit(false);
  }
}
