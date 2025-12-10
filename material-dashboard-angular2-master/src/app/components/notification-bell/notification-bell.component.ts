import { Component, OnInit, OnDestroy } from '@angular/core';
import { NotificationService, NotificationDto } from '../../core/services/notification.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-notification-bell',
  templateUrl: './notification-bell.component.html',
  styleUrls: ['./notification-bell.component.css']
})
export class NotificationBellComponent implements OnInit, OnDestroy {
  unreadCount: number = 0;
  notifications: NotificationDto[] = [];
  showDropdown: boolean = false;
  loading: boolean = false;
  private notificationSubscription?: Subscription;

  constructor(
    public notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadNotifications();
    this.startSignalRConnection();
    this.subscribeToNotifications();
  }

  ngOnDestroy(): void {
    if (this.notificationSubscription) {
      this.notificationSubscription.unsubscribe();
    }
    this.notificationService.stopConnection();
  }

  startSignalRConnection(): void {
    // Get access token from localStorage or your auth service
    const token = localStorage.getItem('token');
    if (token) {
      this.notificationService.startConnection(token);
    }
  }

  subscribeToNotifications(): void {
    // Listen for real-time notifications
    this.notificationSubscription = this.notificationService.notificationReceived$.subscribe({
      next: (notification) => {
        // Add new notification to the list
        this.notifications.unshift(notification);
        if (!notification.isRead) {
          this.unreadCount++;
        }
        
        // Show browser notification if permission granted
        this.showBrowserNotification(notification);
      },
      error: (error) => {
        console.error('Error receiving notification:', error);
      }
    });
  }

  showBrowserNotification(notification: NotificationDto): void {
    if ('Notification' in window && Notification.permission === 'granted') {
      new Notification(notification.title, {
        body: notification.message,
        icon: '/assets/img/logo.png',
        badge: '/assets/img/logo.png'
      });
    }
  }

  requestNotificationPermission(): void {
    if ('Notification' in window && Notification.permission === 'default') {
      Notification.requestPermission();
    }
  }

  loadNotifications(): void {
    this.loading = true;
    this.notificationService.getNotifications(false).subscribe({
      next: (notifications) => {
        this.notifications = notifications;
        this.unreadCount = notifications.filter(n => !n.isRead).length;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading notifications:', error);
        this.loading = false;
      }
    });
  }

  toggleDropdown(): void {
    this.showDropdown = !this.showDropdown;
    if (this.showDropdown) {
      this.loadNotifications();
    }
  }

  closeDropdown(): void {
    this.showDropdown = false;
  }

  markAsRead(notification: NotificationDto, event: Event): void {
    event.stopPropagation();
    
    if (!notification.isRead) {
      this.notificationService.markAsRead(notification.id).subscribe({
        next: () => {
          notification.isRead = true;
          notification.readAt = new Date().toISOString();
          this.unreadCount = Math.max(0, this.unreadCount - 1);
          
          if (notification.actionUrl) {
            this.router.navigate([notification.actionUrl]);
            this.closeDropdown();
          }
        },
        error: (error) => {
          console.error('Error marking notification as read:', error);
        }
      });
    } else if (notification.actionUrl) {
      this.router.navigate([notification.actionUrl]);
      this.closeDropdown();
    }
  }

  markAllAsRead(): void {
    this.notificationService.markAllAsRead().subscribe({
      next: () => {
        this.notifications.forEach(n => {
          n.isRead = true;
          n.readAt = new Date().toISOString();
        });
        this.unreadCount = 0;
      },
      error: (error) => {
        console.error('Error marking all as read:', error);
      }
    });
  }

  viewAllNotifications(): void {
    this.router.navigate(['/notifications']);
    this.closeDropdown();
  }
}
