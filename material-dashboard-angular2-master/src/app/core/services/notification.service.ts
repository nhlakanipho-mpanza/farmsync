import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import * as signalR from '@microsoft/signalr';

export interface NotificationDto {
  id: string;
  userId: string;
  type: string;
  title: string;
  message: string;
  actionUrl?: string;
  data?: string;
  isRead: boolean;
  readAt?: string;
  priority: string;
  createdAt: string;
}

export interface UnreadCountDto {
  unreadCount: number;
}

export interface NotificationSettingDto {
  id: string;
  userId: string;
  emailEnabled: boolean;
  pushEnabled: boolean;
  enabledNotificationTypes: string[];
}

export interface UpdateNotificationSettingDto {
  emailEnabled: boolean;
  pushEnabled: boolean;
  enabledNotificationTypes: string[];
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = `${environment.apiUrl}/api/notifications`;
  private hubConnection?: signalR.HubConnection;
  private notificationReceived = new Subject<NotificationDto>();
  
  // Observable for components to subscribe to
  public notificationReceived$ = this.notificationReceived.asObservable();

  constructor(private http: HttpClient) {}

  // Start SignalR connection
  public startConnection(accessToken: string): void {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      console.log('SignalR already connected');
      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/notifications`, {
        accessTokenFactory: () => accessToken
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR Connected to NotificationHub');
        this.registerNotificationHandler();
      })
      .catch(err => console.error('Error starting SignalR connection:', err));

    // Handle reconnection
    this.hubConnection.onreconnected(() => {
      console.log('SignalR Reconnected');
    });

    this.hubConnection.onreconnecting(() => {
      console.log('SignalR Reconnecting...');
    });

    this.hubConnection.onclose(() => {
      console.log('SignalR Connection closed');
    });
  }

  // Register handler for incoming notifications
  private registerNotificationHandler(): void {
    this.hubConnection?.on('ReceiveNotification', (notification: NotificationDto) => {
      console.log('Real-time notification received:', notification);
      this.notificationReceived.next(notification);
    });
  }

  // Stop SignalR connection
  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop()
        .then(() => console.log('SignalR Disconnected'))
        .catch(err => console.error('Error stopping SignalR connection:', err));
    }
  }

  getNotifications(unreadOnly: boolean = false): Observable<NotificationDto[]> {
    return this.http.get<NotificationDto[]>(`${this.apiUrl}?unreadOnly=${unreadOnly}`);
  }

  getUnreadCount(): Observable<UnreadCountDto> {
    return this.http.get<UnreadCountDto>(`${this.apiUrl}/unread-count`);
  }

  markAsRead(id: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/mark-read`, {});
  }

  markAllAsRead(): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/mark-all-read`, {});
  }

  getSettings(): Observable<NotificationSettingDto> {
    return this.http.get<NotificationSettingDto>(`${this.apiUrl}/settings`);
  }

  updateSettings(dto: UpdateNotificationSettingDto): Observable<NotificationSettingDto> {
    return this.http.put<NotificationSettingDto>(`${this.apiUrl}/settings`, dto);
  }

  getPriorityColor(priority: string): string {
    switch (priority) {
      case 'Urgent': return '#f44336';
      case 'High': return '#ff9800';
      case 'Normal': return '#2196F3';
      case 'Low': return '#9E9E9E';
      default: return '#2196F3';
    }
  }

  getPriorityIcon(priority: string): string {
    switch (priority) {
      case 'Urgent': return 'priority_high';
      case 'High': return 'warning';
      case 'Normal': return 'info';
      case 'Low': return 'info_outline';
      default: return 'notifications';
    }
  }

  getTypeIcon(type: string): string {
    switch (type) {
      case 'AccountCreated': return 'person_add';
      case 'DocumentExpiringSoon': return 'schedule';
      case 'DocumentExpired': return 'error';
      case 'PurchaseOrderStatusChanged': return 'shopping_cart';
      case 'MaintenanceDue': return 'build';
      case 'LeaveApproved': return 'check_circle';
      case 'LeaveRejected': return 'cancel';
      case 'System': return 'settings';
      default: return 'notifications';
    }
  }

  getRelativeTime(dateString: string): string {
    const date = new Date(dateString);
    const now = new Date();
    const seconds = Math.floor((now.getTime() - date.getTime()) / 1000);

    if (seconds < 60) return 'just now';
    if (seconds < 3600) return `${Math.floor(seconds / 60)}m ago`;
    if (seconds < 86400) return `${Math.floor(seconds / 3600)}h ago`;
    if (seconds < 604800) return `${Math.floor(seconds / 86400)}d ago`;
    
    return date.toLocaleDateString();
  }
}
