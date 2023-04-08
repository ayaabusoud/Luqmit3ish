<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="Luqmit3ish.Views.OtherProfilePage"
             Title="{Binding Name}">
    <ContentPage.Content>
        <StackLayout Margin="16,64,16,35">
            <Frame BackgroundColor="White" BorderColor="LightGray" Margin="15,70,15,0" >
                <AbsoluteLayout>
                    <Frame  VerticalOptions="Center" Padding="0"
                            HorizontalOptions="CenterAndExpand"
                            CornerRadius="50" IsClippedToBounds="True" HeightRequest="100" WidthRequest="100" AbsoluteLayout.LayoutBounds="100,-77,100,100">
                        <Image Source="{Binding Photo}" 
                              VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand" Aspect="AspectFill"/>
                    </Frame>
                    <BoxView Margin="0,10,12,10" BackgroundColor="LightGray" AbsoluteLayout.LayoutBounds="0,62,310,21.5"/>
                    <BoxView Margin="0,10,12,10" BackgroundColor="LightGray" AbsoluteLayout.LayoutBounds="0,180,310,21.5"/>
                    <BoxView Margin="0,10,12,10" BackgroundColor="LightGray" AbsoluteLayout.LayoutBounds="0,119,310,21.5"/>
                    <Grid RowDefinitions="50,50,50,35" ColumnDefinitions="0.20*,0.80*" Margin="10,42,0,0" ColumnSpacing="10" >
                        <Label Grid.Row="0" Grid.Column="0" Text="&#xf007;" FontFamily="FontAwesomeIcons"  HorizontalOptions="Start" FontSize="Large" TextColor="Black"/>
                        <Label Grid.Row="0" Grid.Column="1" Text="{Binding Name}" HorizontalOptions="Start" FontSize="Medium"/>
                        <Label Grid.Row="1" Grid.Column="0" Text="&#xf3c5;" FontFamily="FontAwesomeIcons" HorizontalOptions="Start" FontSize="Large" TextColor="Black"/>
                        <Label Grid.Row="1" Grid.Column="1" Text="{Binding Location}" HorizontalOptions="Start" FontSize="Medium"/>
                        <Label Grid.Row="2" Grid.Column="0" Text="&#xf095;" FontFamily="FontAwesomeIcons"  HorizontalOptions="Start" FontSize="Large" TextColor="Black"/>
                        <Label Grid.Row="2" Grid.Column="1" Text="{Binding PhoneNumber}" HorizontalOptions="Start" FontSize="Medium"/>
                        <Label Grid.Row="3" Grid.Column="0" Text="&#xf0e0;" FontFamily="FontAwesomeIcons"  HorizontalOptions="Start" FontSize="Large" TextColor="Black"/>
                        <Label Grid.Row="3" Grid.Column="1" Text="{Binding Email}" HorizontalOptions="Start" FontSize="Medium"/>
                    </Grid>
                </AbsoluteLayout>
            </Frame>
        </StackLayout>
    </ContentPage.Content>
</ContentPage>
