import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-edit-room',
  templateUrl: './edit-room.component.html',
  styleUrls: ['./edit-room.component.css']
})

export class EditRoomComponent implements OnInit {
  @Output() cancelEdit = new EventEmitter();
  @Input() room: any;
  currentUser: any;
  baseUrl = 'http://localhost:5000/api/';

  constructor(public accountService: AccountService, private http: HttpClient) {
    accountService.currentUser$.subscribe((user: User) => (this.currentUser = user));
  }

  ngOnInit(): void {
  }

  editRoom(): void {
    const headers = new HttpHeaders().set(
      'Authorization',
      'Bearer ' + this.currentUser.token
    );

    this.http.post(this.baseUrl + 'room/edit', this.room, {headers}).subscribe();
    this.cancel();
  }

  cancel(): void {
    this.room.editMode = false;
    this.cancelEdit.emit(this.room);
  }
}
