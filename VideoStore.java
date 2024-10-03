import java.util.Random;
import java.util.Scanner;

// Video class to represent a video
class Video {
    String title;
    String director;
    boolean rented;

    // Constructor
    public Video(String title, String director) {
        this.title = title;
        this.director = director;
        this.rented = false;
    }

    // Getter methods
    public String getTitle() {
        return title;
    }

    public boolean isRented() {
        return rented;
    }

    // Setter method
    public void setRented(boolean rented) {
        this.rented = rented;
    }

    @Override
    public String toString() {
        return "Video{" +
                "title='" + title + '\'' +
                ", director='" + director + '\'' +
                ", rented=" + rented +
                '}';
    }
}

// Customer class to represent a customer
class Customer {
    String name;

    // Constructor
    public Customer(String name) {
        this.name = name;
    }

    // Getter method
    public String getName() {
        return name;
    }

    @Override
    public String toString() {
        return "Customer{" +
                "name='" + name + '\'' +
                '}';
    }
}

// Node class for linked lists
class Node<T> {
    T data;
    Node<T> next;
    Node<T> prev;

    // Constructor
    public Node(T data) {
        this.data = data;
        this.next = null;
        this.prev = null;
    }
}

// LinkedList class
class LinkedList<T> {
    Node<T> head;
    Node<T> tail;
    int size;

    // Constructor
    public LinkedList() {
        this.head = null;
        this.tail = null;
        this.size = 0;
    }

    // Method to add a new node to the end of the list
    public void add(T data) {
        Node<T> newNode = new Node<>(data);
        if (head == null) {
            head = newNode;
            tail = newNode;
        } else {
            tail.next = newNode;
            newNode.prev = tail;
            tail = newNode;
        }
        size++;
    }

    // Method to delete a node from the list
    public void delete(T data) {
        if (head == null) {
            return;
        }
        Node<T> current = head;
        while (current != null) {
            if (current.data.equals(data)) {
                if (current == head) {
                    head = head.next;
                    if (head != null) {
                        head.prev = null;
                    } else {
                        tail = null;
                    }
                } else if (current == tail) {
                    tail = tail.prev;
                    tail.next = null;
                } else {
                    current.prev.next = current.next;
                    current.next.prev = current.prev;
                }
                size--;
                return;
            }
            current = current.next;
        }
    }

    // Method to print all elements of the list
    public void print() {
        Node<T> current = head;
        while (current != null) {
            System.out.println(current.data);
            current = current.next;
        }
    }

    // Method to get the size of the list
    public int size() {
        return size;
    }

    // Method to get the element at a specific index
    public T get(int index) {
        if (index < 0 || index >= size) {
            throw new IndexOutOfBoundsException("Index out of bounds: " + index);
        }
        Node<T> current = head;
        for (int i = 0; i < index; i++) {
            current = current.next;
        }
        return current.data;
    }
}

// VideoStore class to manage videos and customers
public class VideoStore {
    private LinkedList<Video> videos;
    private LinkedList<Customer> customers;

    // Constructor
    public VideoStore() {
        videos = new LinkedList<>();
        customers = new LinkedList<>();
    }

    // Method to add a new video
    public void addVideo(String title, String director) {
        Video video = new Video(title, director);
        videos.add(video);
    }

    // Method to delete a video
    public void deleteVideo(String title) {
        Video target = null;
        Node<Video> current = videos.head;
        while (current != null) {
            if (current.data.getTitle().equals(title)) {
                target = current.data;
                break;
            }
            current = current.next;
        }
        if (target != null) {
            videos.delete(target);
        }
    }

    // Method to add a new customer
    public void addCustomer(String name) {
        Customer customer = new Customer(name);
        customers.add(customer);
    }

    // Method to delete a customer
    public void deleteCustomer(String name) {
        Customer target = null;
        Node<Customer> current = customers.head;
        while (current != null) {
            if (current.data.getName().equals(name)) {
                target = current.data;
                break;
            }
            current = current.next;
        }
        if (target != null) {
            customers.delete(target);
        }
    }

    // Method to print all customers
    public void printCustomers() {
        System.out.println("Customers:");
        customers.print();
    }

    // Method to print all videos
    public void printVideos() {
        System.out.println("Videos:");
        videos.print();
    }

    // Method to check if a particular video is in store
    public boolean isVideoInStore(String title) {
        Node<Video> current = videos.head;
        while (current != null) {
            if (current.data.getTitle().equals(title) && !current.data.isRented()) {
                return true;
            }
            current = current.next;
        }
        return false;
    }

    // Method to check out a video
    public void checkOutVideo(String title) {
        Node<Video> current = videos.head;
        while (current != null) {
            if (current.data.getTitle().equals(title) && !current.data.isRented()) {
                current.data.setRented(true);
                return;
            }
            current = current.next;
        }
    }

    // Method to check in a video
    public void checkInVideo(String title) {
        Node<Video> current = videos.head;
        while (current != null) {
            if (current.data.getTitle().equals(title) && current.data.isRented()) {
                current.data.setRented(false);
                return;
            }
            current = current.next;
        }
    }

    // Method to print all in-store videos
    public void printInStoreVideos() {
        System.out.println("In-store videos:");
        Node<Video> current = videos.head;
        while (current != null) {
            if (!current.data.isRented()) {
                System.out.println(current.data);
            }
            current = current.next;
        }
    }

    // Method to print all rent videos
    public void printRentVideos() {
        System.out.println("Rent videos:");
        Node<Video> current = videos.head;
        while (current != null) {
            if (current.data.isRented()) {
                System.out.println(current.data);
            }
            current = current.next;
        }
    }

    // Method to print the videos rented by a customer
    public void printVideosRentedByCustomer(String customerName) {
        System.out.println("Videos rented by " + customerName + ":");
        // Implementation needed
    }

    // Method to generate random transaction requests
    public void generateRequests(int numRequests) {
        Random random = new Random();
        for (int i = 0; i < numRequests; i++) {
            int requestType = random.nextInt(3) + 5; // Randomly select request type between 5 and 7
            switch (requestType) {
                case 5:
                    String title = "Video" + random.nextInt(videos.size());
                    isVideoInStore(title);
                    break;
                case 6:
                    title = "Video" + random.nextInt(videos.size());
                    checkOutVideo(title);
                    break;
                case 7:
                    title = "Video" + random.nextInt(videos.size());
                    checkInVideo(title);
                    break;
            }
        }
    }

    // Main method to run the program
    public static void main(String[] args) {
        VideoStore videoStore = new VideoStore();

        // Stage 1 and 2: Interactive testing
        if (args.length == 0) {
            videoStore.runInteractive();
            return;
        }

        // Stage 3 and 4: Automated testing
        String listType = args[0];
        if (!listType.equals("SLL") && !listType.equals("DLL")) {
            System.out.println("Invalid list type. Please use 'SLL' or 'DLL'.");
            return;
        }

        int numVideos = Integer.parseInt(args[1]);
        int numCustomers = Integer.parseInt(args[2]);
        int numRequests = Integer.parseInt(args[3]);

        // Initialize videos and customers
        for (int i = 0; i < numVideos; i++) {
            videoStore.addVideo("Video" + i, "Director" + i);
        }
        for (int i = 0; i < numCustomers; i++) {
            videoStore.addCustomer("Customer" + i);
        }

        // Generate and process transaction requests
        long startTime = System.nanoTime();
        videoStore.generateRequests(numRequests);
        long endTime = System.nanoTime();

        // Print total service time
        long totalTime = endTime - startTime;
        System.out.println("Total service time: " + totalTime + " nanoseconds");
    }

    // Method to run the program interactively
    public void runInteractive() {
        Scanner scanner = new Scanner(System.in);

        while (true) {
            System.out.println("===========================");
            System.out.println("Select one of the following");
            System.out.println("1: To add a video");
            System.out.println("2: To delete a video");
            System.out.println("3: To add a customer");
            System.out.println("4: To delete a customer");
            System.out.println("5: To check if a particular video is in store");
            System.out.println("6: To check out a video");
            System.out.println("7: To check in a video");
            System.out.println("8: To print all customers");
            System.out.println("9: To print all videos");
            System.out.println("10: To exit");
            System.out.println("===========================");

            int choice = scanner.nextInt();
            scanner.nextLine(); // Consume newline

            switch (choice) {
                case 1:
                    System.out.println("Enter the title of the video:");
                    String title = scanner.nextLine();
                    System.out.println("Enter the director of the video:");
                    String director = scanner.nextLine();
                    addVideo(title, director);
                    break;
                case 2:
                    System.out.println("Enter the title of the video to delete:");
                    title = scanner.nextLine();
                    deleteVideo(title);
                    break;
                case 3:
                    System.out.println("Enter the name of the customer:");
                    String name = scanner.nextLine();
                    addCustomer(name);
                    break;
                case 4:
                    System.out.println("Enter the name of the customer to delete:");
                    name = scanner.nextLine();
                    deleteCustomer(name);
                    break;
                case 5:
                    System.out.println("Enter the title of the video to check:");
                    title = scanner.nextLine();
                    if (isVideoInStore(title)) {
                        System.out.println("Video '" + title + "' is in store.");
                    } else {
                        System.out.println("Video '" + title + "' is not in store.");
                    }
                    break;
                case 6:
                    System.out.println("Enter the title of the video to check out:");
                    title = scanner.nextLine();
                    checkOutVideo(title);
                    break;
                case 7:
                    System.out.println("Enter the title of the video to check in:");
                    title = scanner.nextLine();
                    checkInVideo(title);
                    break;
                case 8:
                    printCustomers();
                    break;
                case 9:
                    printVideos();
                    break;
                case 10:
                    System.out.println("Goodbye!");
                    return;
                default:
                    System.out.println("Invalid choice. Please enter a number from 1 to 10.");
            }
        }
    }
}
